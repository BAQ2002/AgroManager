(function () {
    // Converte string de data (ISO, dd-mm-yyyy ou dd/mm/yyyy) para objeto Date com validação básica.
    function parseDate(value) {
        // Retorna nulo quando valor não existir.
        if (!value) return null;

        // Remove espaços extras para evitar falhas de parsing.
        const normalized = String(value).trim();
        // Detecta formatos locais com separador "/" ou "-": dd/mm/yyyy ou dd-mm-yyyy.
        const localMatch = normalized.match(/^(\d{2})[/-](\d{2})[/-](\d{4})$/);
        if (localMatch) {
            // Extrai dia, mês e ano da string.
            const day = Number(localMatch[1]);
            const month = Number(localMatch[2]);
            const year = Number(localMatch[3]);
            // Cria data usando timezone local para evitar deslocamentos.
            const parsedLocal = new Date(year, month - 1, day);
            // Valida consistência para bloquear datas inválidas como 31/02/2025.
            if (
                parsedLocal.getFullYear() === year &&
                parsedLocal.getMonth() === month - 1 &&
                parsedLocal.getDate() === day
            ) {
                return parsedLocal;
            }
        }

        // Tenta converter formatos nativos (ex.: yyyy-mm-dd da API).
        const parsed = new Date(normalized);
        // Retorna nulo se a data for inválida; caso contrário, retorna a data convertida.
        return Number.isNaN(parsed.getTime()) ? null : parsed;
    }

    // Formata uma data para exibição no padrão brasileiro dd/mm/yyyy.
    function formatDateBr(value) {
        // Converte valor recebido para Date; quando inválido, exibe texto original para não ocultar informação.
        const date = parseDate(value);
        if (!date) return value ?? "";

        // Garante dois dígitos para dia e mês.
        const day = String(date.getDate()).padStart(2, "0");
        const month = String(date.getMonth() + 1).padStart(2, "0");
        const year = String(date.getFullYear());

        // Retorna data formatada no padrão desejado.
        return `${day}/${month}/${year}`;
    }

    // Expõe API pública para reutilização nas páginas que renderizam DataGrid.
    window.DataGrid = {
        parseDate,
        formatDateBr
    };
})();
