(function () {
    const selects = Array.from(document.querySelectorAll(".js-single-enum-select"));

    if (!selects.length) {
        return;
    }

    const getControlParts = (selectElement) => {
        const trigger = selectElement.querySelector(".enum-single-select-trigger");
        const valueInput = selectElement.querySelector("input[type='hidden']");
        const options = Array.from(selectElement.querySelectorAll(".enum-single-select-option"));
        return { trigger, valueInput, options };
    };

    const syncOptionState = (selectElement) => {
        const { trigger, valueInput, options } = getControlParts(selectElement);

        if (!trigger || !valueInput || !options.length) {
            return;
        }

        const currentValue = valueInput.value ?? "";
        let activeOption = options.find((option) => option.dataset.value === currentValue);

        if (!activeOption && currentValue !== "") {
            activeOption = options[0];
            valueInput.value = activeOption.dataset.value ?? "";
        }

        options.forEach((option) => {
            const isActive = option === activeOption;
            option.classList.toggle("active", isActive);
            option.setAttribute("aria-pressed", isActive ? "true" : "false");
        });

        trigger.textContent = activeOption
            ? (activeOption.dataset.label || activeOption.textContent?.trim() || "")
            : "";
        valueInput.dispatchEvent(new Event("input", { bubbles: true }));
        valueInput.dispatchEvent(new Event("change", { bubbles: true }));
    };

    const closeSelect = (selectElement) => {
        const trigger = selectElement.querySelector(".enum-single-select-trigger");
        selectElement.classList.remove("open");
        if (trigger) {
            trigger.setAttribute("aria-expanded", "false");
        }
    };

    const openSelect = (selectElement) => {
        const trigger = selectElement.querySelector(".enum-single-select-trigger");
        selectElement.classList.add("open");
        if (trigger) {
            trigger.setAttribute("aria-expanded", "true");
        }
    };

    selects.forEach((selectElement) => {
        const { trigger, options } = getControlParts(selectElement);

        if (!trigger || !options.length) {
            return;
        }

        syncOptionState(selectElement);

        trigger.addEventListener("click", () => {
            const isOpen = selectElement.classList.contains("open");

            selects.forEach(closeSelect);

            if (!isOpen) {
                openSelect(selectElement);
            }
        });

        options.forEach((option) => {
            option.addEventListener("click", () => {
                const valueInput = selectElement.querySelector("input[type='hidden']");
                if (!valueInput) {
                    return;
                }

                valueInput.value = option.dataset.value ?? "";
                syncOptionState(selectElement);
                closeSelect(selectElement);
            });
        });
    });

    document.addEventListener("click", (event) => {
        selects.forEach((selectElement) => {
            if (!selectElement.contains(event.target)) {
                closeSelect(selectElement);
            }
        });
    });
})();
