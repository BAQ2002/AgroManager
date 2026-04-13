(function () {
    function createIconMarkup(direction, isDouble) {
        const path = direction === "left"
            ? (isDouble ? "M14.8 5.2L9 11l5.8 5.8M10.8 5.2L5 11l5.8 5.8" : "M13.5 5.2L7.7 11l5.8 5.8")
            : (isDouble ? "M9.2 5.2L15 11l-5.8 5.8M5.2 5.2L11 11l-5.8 5.8" : "M8.5 5.2L14.3 11l-5.8 5.8");

        return `<svg viewBox="0 0 20 20" aria-hidden="true"><path d="${path}" /></svg>`;
    }

    function ensureYearButtons(instance) {
        const calendar = instance?.calendarContainer;
        const currentMonth = calendar?.querySelector(".flatpickr-current-month");
        const yearDisplay = currentMonth?.querySelector(".numInputWrapper") || currentMonth?.querySelector("input.cur-year");

        if (!currentMonth || !yearDisplay) {
            return;
        }

        const existingPrevYear = currentMonth.querySelector(".flatpickr-prev-year");
        const existingNextYear = currentMonth.querySelector(".flatpickr-next-year");

        if (!existingPrevYear) {
            const prevYearButton = document.createElement("button");
            prevYearButton.type = "button";
            prevYearButton.className = "flatpickr-prev-year";
            prevYearButton.setAttribute("aria-label", "Ano anterior");
            prevYearButton.innerHTML = createIconMarkup("left", true);
            prevYearButton.addEventListener("click", function () {
                instance.changeYear(-1);
                instance.redraw();
            });
            currentMonth.insertBefore(prevYearButton, yearDisplay);
        }

        if (!existingNextYear) {
            const nextYearButton = document.createElement("button");
            nextYearButton.type = "button";
            nextYearButton.className = "flatpickr-next-year";
            nextYearButton.setAttribute("aria-label", "Próximo ano");
            nextYearButton.innerHTML = createIconMarkup("right", true);
            nextYearButton.addEventListener("click", function () {
                instance.changeYear(1);
                instance.redraw();
            });
            currentMonth.insertBefore(nextYearButton, yearDisplay.nextSibling);
        }
    }

    function updateMonthIcons(instance) {
        const calendar = instance?.calendarContainer;
        const previousMonthButton = calendar?.querySelector(".flatpickr-prev-month");
        const nextMonthButton = calendar?.querySelector(".flatpickr-next-month");

        if (previousMonthButton) {
            previousMonthButton.innerHTML = createIconMarkup("left", false);
        }

        if (nextMonthButton) {
            nextMonthButton.innerHTML = createIconMarkup("right", false);
        }
    }

    function repositionNextMonthButton(instance) {
        const calendar = instance?.calendarContainer;

        if (!calendar) {
            return;
        }

        const nextMonthButton = calendar.querySelector(".flatpickr-next-month");
        const currentMonth = calendar.querySelector(".flatpickr-current-month");
        const yearDisplay = currentMonth?.querySelector(".numInputWrapper") || currentMonth?.querySelector("input.cur-year");

        if (!nextMonthButton || !currentMonth || !yearDisplay || currentMonth.contains(nextMonthButton)) {
            return;
        }

        currentMonth.insertBefore(nextMonthButton, yearDisplay);
    }

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
            onReady: function (_, __, instance) {
                updateMonthIcons(instance);
                repositionNextMonthButton(instance);
                ensureYearButtons(instance);
            },
            onMonthChange: function (_, __, instance) {
                updateMonthIcons(instance);
                repositionNextMonthButton(instance);
                ensureYearButtons(instance);
            },
            onYearChange: function (_, __, instance) {
                ensureYearButtons(instance);
            },
            onChange: function () {
                // Dispara eventos para manter filtros e estados visuais sincronizados.
                element.dispatchEvent(new Event("input", { bubbles: true }));
                element.dispatchEvent(new Event("change", { bubbles: true }));
            }
        });

        element.dataset.datePickerReady = "true";
    });
})();
