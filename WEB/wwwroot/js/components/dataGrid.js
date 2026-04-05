(function () {
    // Converte string de data (ISO, dd-mm-yyyy ou dd/mm/yyyy) para objeto Date com validação básica.
    function parseDate(value) {
        
        if (!value) return null;                                                // Retorna nulo quando valor não existir.

        const normalized = String(value).trim();                                // Remove espaços extras para evitar falhas de parsing.      
        const localMatch = normalized.match(/^(\d{2})[/-](\d{2})[/-](\d{4})$/); // Detecta formatos locais com separador "/" ou "-": dd/mm/yyyy ou dd-mm-yyyy.

        if (localMatch) {
            
            const day = Number(localMatch[1]);   //Extrai dia da string.
            const month = Number(localMatch[2]); //Extrai mês da string.
            const year = Number(localMatch[3]);  //Extrai ano da string.
            
            const parsedLocal = new Date(year, month - 1, day); //Cria data usando timezone local para evitar deslocamentos.

            //Valida consistência para bloquear datas inválidas como 31/02/2025.
            if (
                parsedLocal.getFullYear() === year &&
                parsedLocal.getMonth() === month - 1 &&
                parsedLocal.getDate() === day
            ) {
                return parsedLocal;
            }
        }
      
        const parsed = new Date(normalized);                   //Tenta converter formatos nativos (ex.: yyyy-mm-dd da API).  
        return Number.isNaN(parsed.getTime()) ? null : parsed; //Retorna nulo se a data for inválida; caso contrário, retorna a data convertida.
    }

    //Formata uma data para exibição no padrão brasileiro dd/mm/yyyy.
    function formatDateBr(value) {
        
        const date = parseDate(value);   //Converte valor recebido para Date; quando inválido, exibe texto original para não ocultar informação.
        if (!date) return value ?? "";

        //Garante dois dígitos para dia e mês.
        const day = String(date.getDate()).padStart(2, "0");
        const month = String(date.getMonth() + 1).padStart(2, "0");
        const year = String(date.getFullYear());

       
        return `${day}/${month}/${year}`; //Retorna data formatada no padrão desejado.
    }

    //Expõe API pública para reutilização nas páginas que renderizam DataGrid.
    window.DataGrid = {
        parseDate,
        formatDateBr
    };
})();
