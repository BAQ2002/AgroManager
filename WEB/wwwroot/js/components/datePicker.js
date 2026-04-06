(function () {
    // Inicializa datepickers apenas quando a biblioteca Flatpickr estiver disponível.
    if (!window.flatpickr) {
        return;
    }

    // Usa locale pt-BR para rótulos e nomes de meses/dias.
    if (window.flatpickr.l10ns?.pt) {
        window.flatpickr.localize(window.flatpickr.l10ns.pt);
    }

    // Aplica comportamento de datepicker para todos os inputs marcados com a classe padrão.
    document.querySelectorAll(".js-date-picker").forEach((element) => {
        // Evita inicializar mais de uma vez o mesmo campo.
        if (element.dataset.datePickerReady === "true") {
            return;
        }

        window.flatpickr(element, {
            dateFormat: "Y-m-d",
            allowInput: true,
            disableMobile: true,
            clickOpens: true,
            onChange: function () {
                // Dispara eventos para manter filtros e estados visuais sincronizados.
                element.dispatchEvent(new Event("input", { bubbles: true }));
                element.dispatchEvent(new Event("change", { bubbles: true }));
            }
        });

        element.dataset.datePickerReady = "true";
    });
})();
