(function () {
    // Seleciona todos os controles que participam do comportamento de label flutuante.
    document.querySelectorAll(".js-floating-control").forEach((element) => {
        // Localiza o container visual do campo atual.
        const field = element.closest(".floating-field");
        // Se não existir container, interrompe processamento do elemento atual.
        if (!field) {
            return;
        }

        // Lê do dataset os valores que devem ser tratados como "vazio".
        const emptyValues = new Set(
            // Obtém a string de valores vazios ou fallback para string vazia.
            (element.dataset.emptyValues || "")
                // Divide os valores por vírgula.
                .split(",")
                // Remove espaços extras de cada valor.
                .map((value) => value.trim())
                // Mantém apenas valores não vazios.
                .filter((value) => value !== "")
        );

        // Adiciona string vazia como valor vazio padrão.
        emptyValues.add("");

        // Define função que sincroniza a classe visual de preenchimento.
        const syncFilledState = () => {
            // Lê e normaliza o valor atual do campo.
            const currentValue = (element.value || "").trim();
            // Aplica/remova a classe `filled` conforme o campo possua valor útil.
            field.classList.toggle("filled", !emptyValues.has(currentValue));
        };

        // Recalcula estado ao ocorrer mudança de valor por seleção.
        element.addEventListener("change", syncFilledState);
        // Recalcula estado durante digitação.
        element.addEventListener("input", syncFilledState);
        // Executa sincronização inicial ao carregar a página.
        syncFilledState();
    });
})();
