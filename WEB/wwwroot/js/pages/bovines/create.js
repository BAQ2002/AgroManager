(function () {
    document.querySelectorAll(".js-floating-control").forEach((element) => {
        const field = element.closest(".floating-field");

        const emptyValues = new Set(
            (element.dataset.emptyValues || "")
                .split(",")
                .map((value) => value.trim())
                .filter((value) => value !== "")
        );

        emptyValues.add("");

        const syncFilledState = () => {
            const currentValue = (element.value || "").trim();
            field.classList.toggle("filled", !emptyValues.has(currentValue));
        };

        element.addEventListener("change", syncFilledState);
        element.addEventListener("input", syncFilledState);
        syncFilledState();
    });
})();
