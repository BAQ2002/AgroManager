using MODEL;

namespace BLL
{
    public class LoteRepos
    {

        public void AddAnimal(Guid animalId, DateTimeOffset dataEntrada)
        {
            var newItem = new LoteRegister()
            {
                animalId = animalId,
                dataEntrada = dataEntrada,
                dataSaida = null,
                motivoSaida = null
            };

            _animalsList.Add(newItem);
            Update();
        }

        public void registerExitAnimal(Guid animalId, DateTimeOffset dataSaida, string motivoSaida)
        {
            int index = _animalsList.FindIndex(item => item.animalId == animalId && item.dataSaida == null); //Encontra 

            if (index >= 0)
            {
                var atual = _animalsList[index];
                _animalsList[index] =
                    (
                    atual.animalId,
                    atual.dataEntrada,
                    dataSaida,
                    motivoSaida
                );
            }
        }
    }
}
