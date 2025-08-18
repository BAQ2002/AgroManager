using INFRA;
using MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BLL
{
    // Contrato da BLL exposto ao PL (WinForms)
    public interface IBovineService
    {
        Task<Guid> CadastrarAsync(BovineDto dto, CancellationToken ct = default);
        Task TransferirParaLoteAsync(Guid bovinoId, Guid loteDestinoId, DateTimeOffset quando, CancellationToken ct = default);
        Task RegistrarPesagemAsync(Guid bovinoId, DateOnly data, decimal pesoKg, CancellationToken ct = default);
    }
    public sealed class BovineService : IBovineService
    {
        private readonly BovineRepositoryEF _repo;
        public BovineService(BovineRepositoryEF repo) => _repo = repo;

        public Task AddBovineAsync(BovineEntity e, CancellationToken ct = default) => _repo.AddAsync(e, ct);
        public Task<BovineEntity?> BuscarPorIdAsync(Guid id, CancellationToken ct = default) => _repo.GetByIdAsync(id, ct);
    
        private readonly IBovineRepository _bovinos;
        private readonly IAnimalLoteRepository _animalLotes;
        private readonly IWeightPolicy _weightPolicy;      // regra de negócio (ex.: outliers, frequência mínima)
        private readonly IUnitOfWork _uow;                 // transação/integridade (implementada na INFRA)

        public BovineService(
            IBovineRepository bovinos,
            IAnimalLoteRepository animalLotes,
            IWeightPolicy weightPolicy,
            IUnitOfWork uow)
        {
            _bovinos = bovinos;
            _animalLotes = animalLotes;
            _weightPolicy = weightPolicy;
            _uow = uow;
        }

        public async Task<Guid> CadastrarAsync(BovineDto dto, CancellationToken ct = default)
        {
            // validações de negócio
            var jaExiste = await _bovinos.ExistsByEarringAsync(dto.Earring, ct);
            if (jaExiste) throw new RegraDeNegocioException("Brinco já cadastrado.");

            var entity = dto.ToEntity(); // mapeia DTO -> entidade do domínio

            await _uow.ExecuteAsync(async () =>
            {
                await _bovinos.AddAsync(entity, ct);
                await _animalLotes.AlocarAsync(entity.Id, dto.LoteInicialId, dto.DataEntrada ?? DateTimeOffset.Now, ct);
            }, ct);

            return entity.Id;
        }

        public async Task TransferirParaLoteAsync(Guid bovinoId, Guid loteDestinoId, DateTimeOffset quando, CancellationToken ct = default)
        {
            await _uow.ExecuteAsync(async () =>
            {
                var ativo = await _animalLotes.ObterAlocacaoAtivaAsync(bovinoId, ct)
                           ?? throw new RegraDeNegocioException("Animal não está alocado.");
                if (ativo.LoteId == loteDestinoId) return; // idempotência

                // regra: checar capacidade do lote destino
                await _animalLotes.ValidarCapacidadeAsync(loteDestinoId, ct);

                await _animalLotes.FecharAlocacaoAsync(bovinoId, quando, motivoSaida: "Transferência", ct);
                await _animalLotes.AlocarAsync(bovinoId, loteDestinoId, quando, ct);
            }, ct);
        }

        public async Task RegistrarPesagemAsync(Guid bovinoId, DateOnly data, decimal pesoKg, CancellationToken ct = default)
        {
            // regra: outlier/frequência mínima
            await _weightPolicy.EnsureCanRegisterAsync(bovinoId, data, pesoKg, ct);

            await _uow.ExecuteAsync(async () =>
            {
                await _bovinos.AdicionarPesoAsync(bovinoId, data, pesoKg, ct);
                // (opcional) publicar evento para analytics/IoT/outbox
            }, ct);
        }
    }

}
