(function () {
    // Mantém em memória todos os bovinos carregados da API para filtrar no cliente.
    let allBovines = [];
    // Define unidade de idade padrão usada nos filtros e na renderização.
    let currentAgeUnit = "years";
    let sortState = { key: null, direction: "asc" };

    const grid = window.DataGrid.create({
        rowsSelector: "#rows",
        countSelector: "#count",
        pageSizeSelector: "#bovinesPageSize",
        paginationInfoSelector: "#bovinesPageInfo",
        prevPageSelector: "#bovinesPrevPage",
        nextPageSelector: "#bovinesNextPage",
        data: [],
        rowTemplate: (x) => `
          <tr>
            <td>${x.name ?? ""}</td>
            <td>${x.gender ?? ""}</td>
            <td>${x.origin ?? ""}</td>
            <td>${formatBirthDate(x.birthDate)}</td>
            <td>${formatAgeByUnit(x)}</td>
            <td>${x.maritalStatus ?? ""}</td>
            <td>${x.cattleType ?? ""}</td>
            <td>
                <a class="badge primary-button button-with-icon bovine-edit-action" href="/bovines/edit/${x.id}" aria-label="Editar ${x.name ?? "bovino"}" title="Editar ${x.name ?? "bovino"}">
                    <svg viewBox="0 0 24 24" aria-hidden="true" focusable="false">
                        <path d="M3 21h3.75L17.81 9.94a2 2 0 0 0 0-2.83l-.92-.92a2 2 0 0 0-2.83 0L3 17.25V21z"></path>
                        <path d="M13.5 6.5l4 4"></path>
                    </svg>
                    <span>Editar</span>
                </a>
            </td>
          </tr>
        `
    });

    // Sincroniza o estado visual de preenchimento de um único campo flutuante.
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

    // Sincroniza estado visual de todos os campos flutuantes da tela.
    function syncFloatingFields() {
        document.querySelectorAll(".js-floating-control").forEach(syncFloatingField);
    }

    // Converte string de data para Date reutilizando utilitário compartilhado do componente DataGrid.
    function toDate(value) {
        return window.DataGrid?.parseDate(value) ?? (value ? new Date(value) : null);
    }

    // Formata data no padrão brasileiro dd/mm/yyyy reutilizando utilitário compartilhado do componente DataGrid.
    function formatBirthDate(value) {
        return window.DataGrid?.formatDateBr(value) ?? (value ?? "");
    }

    // Retorna os valores selecionados (em minúsculo) de um grupo de checkboxes.
    function getCheckedValues(containerId) {
        return Array.from(document.querySelectorAll(`#${containerId} input[type='checkbox']:checked`))
            .map((x) => x.value.toLowerCase());
    }

    // Obtém idade de um item de acordo com a unidade ativa.
    function getAgeByUnit(item) {
        if (currentAgeUnit === "days") return item.ageInDays;
        if (currentAgeUnit === "months") return item.ageInMonths;
        return item.ageInYears ?? item.age;
    }

    // Retorna rótulo textual da unidade de idade atual.
    function getAgeUnitLabel() {
        if (currentAgeUnit === "days") return "dias";
        if (currentAgeUnit === "months") return "meses";
        return "anos";
    }

    // Formata idade de um item para exibição amigável na tabela.
    function formatAgeByUnit(item) {
        const value = getAgeByUnit(item);
        if (value === null || value === undefined || value === "") return "";
        return `${value} ${getAgeUnitLabel()}`;
    }

    // Atualiza texto do cabeçalho que indica unidade ativa de idade.
    function updateAgeUnitLabel() {
        const label = document.getElementById("ageUnitLabel");
        if (label) {
            label.textContent = getAgeUnitLabel();
        }
    }

    // Aplica todos os filtros selecionados sobre a lista em memória.
    function applyFilters() {
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

        const filtered = allBovines.filter((x) => {
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

        grid.setData(sortData(filtered));
    }

    // Ordena os dados filtrados usando somente a coluna ativa no momento.
    function sortData(items) {
        if (!sortState.key) return [...items];

        return [...items].sort((a, b) => {
            const factor = sortState.direction === "desc" ? -1 : 1;
            const left = getSortableValue(a, sortState.key);
            const right = getSortableValue(b, sortState.key);

            if (left === right) return 0;
            return left > right ? factor : -factor;
        });
    }

    function getSortableValue(item, key) {
        if (key === "birthDate") {
            return toDate(item.birthDate)?.getTime() ?? Number.MIN_SAFE_INTEGER;
        }

        if (key === "age") {
            const age = getAgeByUnit(item);
            return age === null || age === undefined || age === "" ? Number.MIN_SAFE_INTEGER : Number(age);
        }

        return (item[key] ?? "").toString().toLowerCase();
    }

    function getSortHeaders() {
        return Array.from(document.querySelectorAll(".datagrid thead th[data-sort-key]"));
    }

    function setActiveSort(key, direction = sortState.direction || "asc") {
        sortState = { key, direction };
        syncSortDirectionMenus();
    }

    function syncSortDirectionMenus() {
        getSortHeaders().forEach((header) => {
            const isActiveHeader = header.dataset.sortKey === sortState.key;
            const direction = isActiveHeader ? sortState.direction : "asc";
            const directionBtn = header.querySelector('.datagrid-sort-direction-btn, [data-role="direction"]');

            header.classList.toggle("is-sort-active", isActiveHeader);
            directionBtn?.classList.toggle("is-active", isActiveHeader);
            directionBtn?.setAttribute("aria-pressed", isActiveHeader.toString());

            header.querySelectorAll('[data-menu="direction"] .enum-combo-box-option').forEach((option) => {
                option.classList.toggle("active", isActiveHeader && option.dataset.value === direction);
            });
        });
    }

    function closeSortMenus() {
        document.querySelectorAll('.datagrid-sort-menu.open').forEach((menu) => {
            menu.classList.remove("open");
            menu.previousElementSibling
                ?.querySelector('.datagrid-sort-direction-btn, [data-role="direction"]')
                ?.setAttribute("aria-expanded", "false");
        });
    }

    function positionSortMenu(directionBtn, directionMenu) {
        directionMenu.style.left = `${directionBtn.offsetLeft}px`;
        directionMenu.style.top = `${directionBtn.offsetTop + directionBtn.offsetHeight + 1}px`;
        directionMenu.style.right = "auto";
    }

    function wireSortMenus() {
        const headers = getSortHeaders();
        syncSortDirectionMenus();

        headers.forEach((header) => {
            const directionBtn = header.querySelector('.datagrid-sort-direction-btn, [data-role="direction"]');
            const directionMenu = header.querySelector('[data-menu="direction"]');

            if (!directionBtn || !directionMenu) return;

            directionBtn.setAttribute("aria-expanded", "false");

            directionBtn.addEventListener("click", (event) => {
                event.stopPropagation();
                const isOpen = directionMenu.classList.contains("open");

                closeSortMenus();

                if (!isOpen) {
                    positionSortMenu(directionBtn, directionMenu);
                    directionMenu.classList.add("open");
                    directionBtn.setAttribute("aria-expanded", "true");
                }
            });

            directionMenu.addEventListener("click", (event) => {
                const option = event.target.closest(".enum-combo-box-option");
                if (!option) return;

                setActiveSort(header.dataset.sortKey, option.dataset.value);
                closeSortMenus();
                applyFilters();
            });
        });

        document.addEventListener("click", closeSortMenus);
    }

    // Conecta eventos dos filtros e da ação de limpar filtros.
    function wireFilters() {
        const selectors = [
            "#filterName",
            "#filterBirthDateMin",
            "#filterBirthDateMax",
            "#filterAgeMin",
            "#filterAgeMax"
        ];

        selectors
            .flatMap((selector) => Array.from(document.querySelectorAll(selector)))
            .forEach((element) => {
                element.addEventListener("input", () => {
                    syncFloatingField(element);
                    applyFilters();
                });

                element.addEventListener("change", () => {
                    syncFloatingField(element);
                    applyFilters();
                });
            });

        window.EnumCheckBox?.init();
        document.querySelectorAll(".enum-check-box").forEach((selectElement) => {
            selectElement.addEventListener("enum-check-box:change", applyFilters);
        });

        wireAgeUnitSelect();

        const clearFiltersButton = document.querySelector('[data-action="clear-filters"]');

        clearFiltersButton?.addEventListener("click", () => {
            document.getElementById("filterName").value = "";
            document.getElementById("filterBirthDateMin")._flatpickr?.clear();
            document.getElementById("filterBirthDateMin").value = "";
            document.getElementById("filterBirthDateMax")._flatpickr?.clear();
            document.getElementById("filterBirthDateMax").value = "";
            document.getElementById("filterAgeMin").value = "";
            document.getElementById("filterAgeMax").value = "";

            document.querySelectorAll(".enum-check-box-menu input[type='checkbox']").forEach((x) => {
                x.checked = false;
            });

            window.EnumCheckBox?.syncAll();
            syncFloatingFields();
            applyFilters();
        });

        syncFloatingFields();
    }

    // Conecta eventos de abertura e seleção da unidade de idade.
    function wireAgeUnitSelect() {
        const select = document.getElementById("ageUnitSelect");
        const trigger = document.getElementById("ageUnitSelect");
        const options = Array.from(select.querySelectorAll(".enum-combo-box-option"));

        trigger.addEventListener("click", (event) => {
            event.stopPropagation();
            const isOpen = select.classList.contains("open");
            select.classList.toggle("open", !isOpen);
            trigger.setAttribute("aria-expanded", (!isOpen).toString());
        });

        options.forEach((option) => {
            option.addEventListener("click", () => {
                currentAgeUnit = option.dataset.unit;
                options.forEach((item) => item.classList.remove("active"));
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

    // Busca dados da API e inicializa a primeira renderização da tabela.
    async function load() {
        const res = await fetch("/api/bovines");
        allBovines = await res.json();
        applyFilters();
    }

    wireFilters();
    wireSortMenus();
    load();
})();
