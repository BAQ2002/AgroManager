window.AgroManager = window.AgroManager || {};

window.AgroManager.syncFloatingField = function (element) {
    const field = element.closest(".floating-field");
    if (!field) return;

    const emptyValues = new Set(
        (element.dataset.emptyValues || "")
            .split(",")
            .map((value) => value.trim())
            .filter((value) => value !== "")
    );

    emptyValues.add("");

    const currentValue = (element.value || "").trim();
    field.classList.toggle("filled", !emptyValues.has(currentValue));
};

window.AgroManager.syncFloatingFields = function () {
    document.querySelectorAll(".js-floating-control").forEach((element) => {
        window.AgroManager.syncFloatingField(element);
    });
};

window.AgroManager.initializeFloatingControls = function (root = document) {
    root.querySelectorAll(".js-floating-control").forEach((element) => {
        if (element.dataset.floatingBound === "true") return;

        const syncFilledState = () => window.AgroManager.syncFloatingField(element);

        element.addEventListener("change", syncFilledState);
        element.addEventListener("input", syncFilledState);
        element.dataset.floatingBound = "true";

        syncFilledState();
    });
};

window.AgroManager.initializeDatePickers = function (root = document) {
    if (typeof flatpickr !== "function") return;

    const locale = flatpickr.l10ns.pt || flatpickr.l10ns.default;

    root.querySelectorAll(".js-date-picker").forEach((element) => {
        if (element._flatpickr) return;

        flatpickr(element, {
            locale,
            dateFormat: "Y-m-d",
            altInput: true,
            altFormat: "d/m/Y",
            allowInput: true,
            disableMobile: true,
            monthSelectorType: "static",
            prevArrow:
                "<svg viewBox='0 0 24 24' aria-hidden='true' focusable='false'><path d='M15 18l-6-6 6-6'></path></svg>",
            nextArrow:
                "<svg viewBox='0 0 24 24' aria-hidden='true' focusable='false'><path d='M9 6l6 6-6 6'></path></svg>",
            onReady: function (_, __, instance) {
                instance.altInput.classList.add("date-picker-input");
                instance.altInput.classList.add("js-date-picker-alt");
                if (element.classList.contains("floating-field-input")) {
                    instance.altInput.classList.add("floating-field-input");
                    instance.altInput.classList.add("js-floating-control");
                    window.AgroManager.initializeFloatingControls(instance.altInput.parentElement);
                    window.AgroManager.syncFloatingField(instance.altInput);
                }

                if (element.classList.contains("am-input")) {
                    instance.altInput.classList.add("am-input");
                }

                if (element.disabled) {
                    instance.altInput.disabled = true;
                }
            },
            onChange: function (_, __, instance) {
                const target = instance.altInput || element;
                window.AgroManager.syncFloatingField(target);
                target.dispatchEvent(new Event("input", { bubbles: true }));
                target.dispatchEvent(new Event("change", { bubbles: true }));
            },
            onValueUpdate: function (_, __, instance) {
                const target = instance.altInput || element;
                window.AgroManager.syncFloatingField(target);
            }
        });
    });
};

document.addEventListener("DOMContentLoaded", function () {
    window.AgroManager.initializeFloatingControls();
    window.AgroManager.initializeDatePickers();
});
