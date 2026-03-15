(function () {
    let allBovines = [];
    let currentAgeUnit = "years";

    function syncFloatingField(element) {
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
    }

    function syncFloatingFields() {
        document.querySelectorAll(".js-floating-control").forEach(syncFloatingField);
    }

    function syncEnumFloatingField(selectElement) {
        const field = selectElement.closest(".floating-field");
        if (!field) return;

        const checked = selectElement.querySelectorAll("input[type='checkbox']:checked").length;
        field.classList.toggle("filled", checked > 0);
    }

    function toDate(value) {
        if (!value) return null;
        const parsed = new Date(value);
        return Number.isNaN(parsed.getTime()) ? null : parsed;
    }

    function getCheckedValues(containerId) {
        return Array.from(document.querySelectorAll(`#${containerId} input[type='checkbox']:checked`)).map(x => x.value.toLowerCase());
    }

    function getAgeByUnit(item) {
        if (currentAgeUnit === "days") return item.ageInDays;
        if (currentAgeUnit === "months") return item.ageInMonths;
        return item.ageInYears ?? item.age;
    }

    function getAgeUnitLabel() {
        if (currentAgeUnit === "days") return "dias";
        if (currentAgeUnit === "months") return "meses";
        return "anos";
    }

    function formatAgeByUnit(item) {
        const value = getAgeByUnit(item);
        if (value === null || value === undefined || value === "") return "";
        return `${value} ${getAgeUnitLabel()}`;
    }

    function updateAgeUnitLabel() {
        const label = document.getElementById("ageUnitLabel");
        if (label) {
            label.textContent = getAgeUnitLabel();
        }
    }

    function renderRows(data) {
        document.getElementById("rows").innerHTML = data.map(x => `
          <tr>
            <td>${x.name ?? ""}</td>
            <td>${x.gender ?? ""}</td>
            <td>${x.origin ?? ""}</td>
            <td>${x.birthDate ?? ""}</td>
            <td>${formatAgeByUnit(x)}</td>
            <td>${x.maritalStatus ?? ""}</td>
            <td>${x.cattleType ?? ""}</td>
            <td><a class="badge am-link-plain" href="/bovines/edit/${x.id}">Editar</a></td>
          </tr>
        `).join("");

        document.getElementById("count").innerText = `${data.length} registro(s) exibidos`;
    }

    function applyFilters() {
        const toggleSwitch = document.getElementById("toggleFilters");
        if (toggleSwitch && !toggleSwitch.checked) {
            renderRows(allBovines);
            return;
        }

        const nameFilter = document.getElementById("filterName").value.trim().toLowerCase();
        const genders = getCheckedValues("filterGender");
        const origins = getCheckedValues("filterOrigin");
        const maritalStatuses = getCheckedValues("filterMaritalStatus");
        const cattleTypes = getCheckedValues("filterCattleType");

        const birthDateMin = toDate(document.getElementById("filterBirthDateMin").value);
        const birthDateMax = toDate(document.getElementById("filterBirthDateMax").value);

        const ageMinRaw = document.getElementById("filterAgeMin").value;
        const ageMaxRaw = document.getElementById("filterAgeMax").value;
        const ageMin = ageMinRaw === "" ? null : Number(ageMinRaw);
        const ageMax = ageMaxRaw === "" ? null : Number(ageMaxRaw);

        const filtered = allBovines.filter(x => {
            const name = (x.name ?? "").toLowerCase();
            const gender = (x.gender ?? "").toLowerCase();
            const origin = (x.origin ?? "").toLowerCase();
            const maritalStatus = (x.maritalStatus ?? "").toLowerCase();
            const cattleType = (x.cattleType ?? "").toLowerCase();
            const birthDate = toDate(x.birthDate);
            const ageRaw = getAgeByUnit(x);
            const age = ageRaw === null || ageRaw === undefined || ageRaw === "" ? null : Number(ageRaw);

            if (nameFilter && !name.includes(nameFilter)) return false;
            if (genders.length && !genders.includes(gender)) return false;
            if (origins.length && !origins.includes(origin)) return false;
            if (maritalStatuses.length && !maritalStatuses.includes(maritalStatus)) return false;
            if (cattleTypes.length && !cattleTypes.includes(cattleType)) return false;

            if (birthDateMin && (!birthDate || birthDate < birthDateMin)) return false;
            if (birthDateMax && (!birthDate || birthDate > birthDateMax)) return false;

            if (ageMin !== null && (age === null || age < ageMin)) return false;
            if (ageMax !== null && (age === null || age > ageMax)) return false;

            return true;
        });

        renderRows(filtered);
    }

    function wireFilters() {
        const selectors = [
            "#filterName",
            "#filterBirthDateMin",
            "#filterBirthDateMax",
            "#filterAgeMin",
            "#filterAgeMax",
            "#filterGender input[type='checkbox']",
            "#filterOrigin input[type='checkbox']",
            "#filterMaritalStatus input[type='checkbox']",
            "#filterCattleType input[type='checkbox']"
        ];

        selectors
            .flatMap(selector => Array.from(document.querySelectorAll(selector)))
            .forEach(element => {
                element.addEventListener("input", () => {
                    syncFloatingField(element);
                    applyFilters();
                });

                element.addEventListener("change", () => {
                    syncFloatingField(element);
                    applyFilters();
                });
            });

        wireEnumSelects();
        wireAgeUnitSelect();
        wireFiltersPanelToggle();

        document.getElementById("clearFilters").addEventListener("click", () => {
            document.getElementById("filterName").value = "";
            document.getElementById("filterBirthDateMin").value = "";
            document.getElementById("filterBirthDateMax").value = "";
            document.getElementById("filterAgeMin").value = "";
            document.getElementById("filterAgeMax").value = "";

            document.querySelectorAll(".enum-select-menu input[type='checkbox']").forEach(x => x.checked = false);
            updateEnumSelectLabels();
            syncFloatingFields();

            applyFilters();
        });

        syncFloatingFields();
    }

    function wireAgeUnitSelect() {
        const select = document.getElementById("ageUnitSelect");
        const trigger = document.getElementById("ageUnitTrigger");
        const options = Array.from(select.querySelectorAll(".age-unit-option"));

        trigger.addEventListener("click", (event) => {
            event.stopPropagation();
            const isOpen = select.classList.contains("open");
            select.classList.toggle("open", !isOpen);
            trigger.setAttribute("aria-expanded", (!isOpen).toString());
        });

        options.forEach(option => {
            option.addEventListener("click", () => {
                currentAgeUnit = option.dataset.unit;
                options.forEach(item => item.classList.remove("active"));
                option.classList.add("active");
                select.classList.remove("open");
                trigger.setAttribute("aria-expanded", "false");
                updateAgeUnitLabel();
                applyFilters();
            });
        });

        document.addEventListener("click", () => {
            select.classList.remove("open");
            trigger.setAttribute("aria-expanded", "false");
        });

        updateAgeUnitLabel();
    }

    function wireFiltersPanelToggle() {
        const toggleSwitch = document.getElementById("toggleFilters");
        const filtersPanel = document.getElementById("filtersPanel");
        const filtersFieldset = document.getElementById("filtersFieldset");
        const clearFiltersButton = document.getElementById("clearFilters");

        const syncFiltersState = () => {
            const isEnabled = toggleSwitch.checked;
            filtersPanel.classList.toggle("collapsed", !isEnabled);
            filtersFieldset.disabled = !isEnabled;
            clearFiltersButton.disabled = !isEnabled;
            toggleSwitch.setAttribute("aria-expanded", isEnabled.toString());
            applyFilters();
        };

        toggleSwitch.addEventListener("change", syncFiltersState);
        syncFiltersState();
    }

    function updateEnumSelectLabel(selectElement) {
        const trigger = selectElement.querySelector(".enum-select-trigger");
        const checked = selectElement.querySelectorAll("input[type='checkbox']:checked").length;

        if (checked === 0) {
            trigger.textContent = "";
            syncEnumFloatingField(selectElement);
            return;
        }

        trigger.textContent = checked === 1 ? "1 selecionado" : `${checked} selecionados`;
        syncEnumFloatingField(selectElement);
    }

    function updateEnumSelectLabels() {
        document.querySelectorAll(".enum-select").forEach(updateEnumSelectLabel);
    }

    function wireEnumSelects() {
        document.querySelectorAll(".enum-select").forEach(selectElement => {
            const trigger = selectElement.querySelector(".enum-select-trigger");

            selectElement.addEventListener("click", (event) => {
                event.stopPropagation();
            });

            trigger.addEventListener("click", (event) => {
                event.stopPropagation();
                const isOpen = selectElement.classList.contains("open");

                document.querySelectorAll(".enum-select.open").forEach(item => {
                    item.classList.remove("open");
                    item.querySelector(".enum-select-trigger").setAttribute("aria-expanded", "false");
                });

                if (!isOpen) {
                    selectElement.classList.add("open");
                    trigger.setAttribute("aria-expanded", "true");
                }
            });

            selectElement.querySelectorAll("input[type='checkbox']").forEach(checkbox => {
                checkbox.addEventListener("change", () => updateEnumSelectLabel(selectElement));
            });

            updateEnumSelectLabel(selectElement);
        });

        document.addEventListener("click", () => {
            document.querySelectorAll(".enum-select.open").forEach(selectElement => {
                selectElement.classList.remove("open");
                selectElement.querySelector(".enum-select-trigger").setAttribute("aria-expanded", "false");
            });
        });
    }

    async function load() {
        const res = await fetch("/api/bovines");
        allBovines = await res.json();
        renderRows(allBovines);
    }

    wireFilters();
    load();
})();
