(function () {
    // Mantém em memória todos os bovinos carregados da API para filtrar no cliente.
    let allBovines = [];
    // Define unidade de idade padrão usada nos filtros e na renderização.
    let currentAgeUnit = "years";

    // Sincroniza o estado visual de preenchimento de um único campo flutuante.
    function syncFloatingField(element) {
        // Obtém container visual do campo.
        const field = element.closest(".floating-field");
        // Sai da função se não houver container para atualizar.
        if (!field) return;

        // Cria conjunto de valores que devem ser tratados como vazios.
        const emptyValues = new Set(
            // Lê valores vazios configurados no dataset.
            (element.dataset.emptyValues || "")
                // Separa string por vírgula.
                .split(",")
                // Remove espaços extras de cada item.
                .map((value) => value.trim())
                // Filtra entradas realmente vazias.
                .filter((value) => value !== "")
        );

        // Garante que string vazia seja sempre considerada valor vazio.
        emptyValues.add("");

        // Lê valor atual do campo com fallback seguro.
        const currentValue = (element.value || "").trim();
        // Ativa classe `filled` quando valor não pertence ao conjunto de vazios.
        field.classList.toggle("filled", !emptyValues.has(currentValue));
    }

    // Sincroniza estado visual de todos os campos flutuantes da tela.
    function syncFloatingFields() {
        // Executa sincronização individual para cada controle mapeado.
        document.querySelectorAll(".js-floating-control").forEach(syncFloatingField);
    }

    // Sincroniza estado visual de um enum-select baseado em quantidade marcada.
    function syncEnumFloatingField(selectElement) {
        // Obtém container visual do select customizado.
        const field = selectElement.closest(".floating-field");
        // Se não houver container, interrompe função.
        if (!field) return;

        // Conta quantos checkboxes estão marcados no componente.
        const checked = selectElement.querySelectorAll("input[type='checkbox']:checked").length;
        // Marca como preenchido quando houver ao menos uma seleção.
        field.classList.toggle("filled", checked > 0);
    }

    // Converte string de data para objeto Date com validação básica.
    function toDate(value) {
        // Retorna nulo quando valor não existir.
        if (!value) return null;
        // Tenta converter string para data.
        const parsed = new Date(value);
        // Retorna nulo se a data for inválida; caso contrário, retorna a data convertida.
        return Number.isNaN(parsed.getTime()) ? null : parsed;
    }

    // Retorna os valores selecionados (em minúsculo) de um grupo de checkboxes.
    function getCheckedValues(containerId) {
        // Coleta checkboxes marcados dentro do container informado e transforma em array de valores.
        return Array.from(document.querySelectorAll(`#${containerId} input[type='checkbox']:checked`)).map((x) => x.value.toLowerCase());
    }

    // Obtém idade de um item de acordo com a unidade ativa.
    function getAgeByUnit(item) {
        // Se unidade ativa for dias, retorna campo correspondente.
        if (currentAgeUnit === "days") return item.ageInDays;
        // Se unidade ativa for meses, retorna campo correspondente.
        if (currentAgeUnit === "months") return item.ageInMonths;
        // Caso padrão: retorna anos quando disponível com fallback para `age`.
        return item.ageInYears ?? item.age;
    }

    // Retorna rótulo textual da unidade de idade atual.
    function getAgeUnitLabel() {
        // Mapeia unidade "days" para texto em português.
        if (currentAgeUnit === "days") return "dias";
        // Mapeia unidade "months" para texto em português.
        if (currentAgeUnit === "months") return "meses";
        // Retorna texto padrão para anos.
        return "anos";
    }

    // Formata idade de um item para exibição amigável na tabela.
    function formatAgeByUnit(item) {
        // Obtém valor da idade na unidade selecionada.
        const value = getAgeByUnit(item);
        // Para valores nulos/vazios, retorna string vazia para não poluir tabela.
        if (value === null || value === undefined || value === "") return "";
        // Combina número e unidade em uma única string.
        return `${value} ${getAgeUnitLabel()}`;
    }

    // Atualiza texto do cabeçalho que indica unidade ativa de idade.
    function updateAgeUnitLabel() {
        // Obtém elemento de rótulo da unidade no DOM.
        const label = document.getElementById("ageUnitLabel");
        // Atualiza apenas quando elemento existir na página.
        if (label) {
            label.textContent = getAgeUnitLabel();
        }
    }

    // Renderiza linhas da tabela de bovinos e contador de resultados.
    function renderRows(data) {
        // Gera HTML das linhas dinamicamente a partir do array recebido.
        document.getElementById("rows").innerHTML = data.map((x) => `
          <tr>
            <td>${x.name ?? ""}</td>
            <td>${x.gender ?? ""}</td>
            <td>${x.origin ?? ""}</td>
            <td>${x.birthDate ?? ""}</td>
            <td>${formatAgeByUnit(x)}</td>
            <td>${x.maritalStatus ?? ""}</td>
            <td>${x.cattleType ?? ""}</td>
            <td>
                <a class="badge am-link-plain primary-button button-with-icon bovine-edit-action" href="/bovines/edit/${x.id}" aria-label="Editar ${x.name ?? "bovino"}" title="Editar ${x.name ?? "bovino"}">
                    <svg viewBox="0 0 24 24" aria-hidden="true" focusable="false">
                        <path d="M3 21h3.75L17.81 9.94a2 2 0 0 0 0-2.83l-.92-.92a2 2 0 0 0-2.83 0L3 17.25V21z"></path>
                        <path d="M13.5 6.5l4 4"></path>
                    </svg>
                    <span>Editar</span>
                </a>
            </td>
          </tr>
        `).join("");

        // Atualiza contador com total de linhas renderizadas no momento.
        document.getElementById("count").innerText = `${data.length} registro(s) exibidos`;
    }

    // Aplica todos os filtros selecionados sobre a lista em memória.
    function applyFilters() {
        // Lê filtro textual de nome.
        const nameFilter = document.getElementById("filterName").value.trim().toLowerCase();
        // Lê gêneros selecionados.
        const genders = getCheckedValues("filterGender");
        // Lê origens selecionadas.
        const origins = getCheckedValues("filterOrigin");
        // Lê estados civis selecionados.
        const maritalStatuses = getCheckedValues("filterMaritalStatus");
        // Lê tipos de gado selecionados.
        const cattleTypes = getCheckedValues("filterCattleType");

        // Converte data mínima informada para Date.
        const birthDateMin = toDate(document.getElementById("filterBirthDateMin").value);
        // Converte data máxima informada para Date.
        const birthDateMax = toDate(document.getElementById("filterBirthDateMax").value);

        // Lê idade mínima em formato bruto de string.
        const ageMinRaw = document.getElementById("filterAgeMin").value;
        // Lê idade máxima em formato bruto de string.
        const ageMaxRaw = document.getElementById("filterAgeMax").value;
        // Converte idade mínima para número ou nulo.
        const ageMin = ageMinRaw === "" ? null : Number(ageMinRaw);
        // Converte idade máxima para número ou nulo.
        const ageMax = ageMaxRaw === "" ? null : Number(ageMaxRaw);

        // Filtra coleção completa de bovinos usando todas as regras ativas.
        const filtered = allBovines.filter((x) => {
            // Normaliza nome para comparação case-insensitive.
            const name = (x.name ?? "").toLowerCase();
            // Normaliza gênero para comparação case-insensitive.
            const gender = (x.gender ?? "").toLowerCase();
            // Normaliza origem para comparação case-insensitive.
            const origin = (x.origin ?? "").toLowerCase();
            // Normaliza estado civil para comparação case-insensitive.
            const maritalStatus = (x.maritalStatus ?? "").toLowerCase();
            // Normaliza tipo de gado para comparação case-insensitive.
            const cattleType = (x.cattleType ?? "").toLowerCase();
            // Converte data de nascimento do item para Date.
            const birthDate = toDate(x.birthDate);
            // Obtém idade na unidade ativa.
            const ageRaw = getAgeByUnit(x);
            // Normaliza idade numérica para comparações de faixa.
            const age = ageRaw === null || ageRaw === undefined || ageRaw === "" ? null : Number(ageRaw);

            // Rejeita item quando filtro de nome não for atendido.
            if (nameFilter && !name.includes(nameFilter)) return false;
            // Rejeita item quando gênero não estiver entre os selecionados.
            if (genders.length && !genders.includes(gender)) return false;
            // Rejeita item quando origem não estiver entre as selecionadas.
            if (origins.length && !origins.includes(origin)) return false;
            // Rejeita item quando estado civil não estiver entre os selecionados.
            if (maritalStatuses.length && !maritalStatuses.includes(maritalStatus)) return false;
            // Rejeita item quando tipo de gado não estiver entre os selecionados.
            if (cattleTypes.length && !cattleTypes.includes(cattleType)) return false;

            // Rejeita item quando data for menor que o mínimo permitido.
            if (birthDateMin && (!birthDate || birthDate < birthDateMin)) return false;
            // Rejeita item quando data for maior que o máximo permitido.
            if (birthDateMax && (!birthDate || birthDate > birthDateMax)) return false;

            // Rejeita item quando idade for menor que o mínimo permitido.
            if (ageMin !== null && (age === null || age < ageMin)) return false;
            // Rejeita item quando idade for maior que o máximo permitido.
            if (ageMax !== null && (age === null || age > ageMax)) return false;

            // Mantém item quando passou por todas as validações.
            return true;
        });

        // Renderiza tabela apenas com itens filtrados.
        renderRows(filtered);
    }

    // Conecta eventos dos filtros e da ação de limpar filtros.
    function wireFilters() {
        // Lista seletores de elementos que disparam reaplicação dos filtros.
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

        // Expande seletores em elementos e registra listeners de input/change.
        selectors
            .flatMap((selector) => Array.from(document.querySelectorAll(selector)))
            .forEach((element) => {
                // Em entrada de dados, atualiza visual do campo e reaplica filtros.
                element.addEventListener("input", () => {
                    syncFloatingField(element);
                    applyFilters();
                });

                // Em mudança confirmada, atualiza visual do campo e reaplica filtros.
                element.addEventListener("change", () => {
                    syncFloatingField(element);
                    applyFilters();
                });
            });

        // Conecta interações dos enum-selects de múltipla escolha.
        wireEnumSelects();
        // Conecta interação do seletor de unidade de idade.
        wireAgeUnitSelect();

        // Localiza botão responsável por limpar filtros.
        const clearFiltersButton = document.querySelector("[data-action=\"clear-filters\"]");

        // Registra ação de reset completo dos filtros quando botão for clicado.
        clearFiltersButton?.addEventListener("click", () => {
            // Limpa filtro de nome.
            document.getElementById("filterName").value = "";
            // Limpa data mínima de nascimento.
            document.getElementById("filterBirthDateMin").value = "";
            // Limpa data máxima de nascimento.
            document.getElementById("filterBirthDateMax").value = "";
            // Limpa idade mínima.
            document.getElementById("filterAgeMin").value = "";
            // Limpa idade máxima.
            document.getElementById("filterAgeMax").value = "";

            // Desmarca todos os checkboxes de enum-select.
            document.querySelectorAll(".enum-select-menu input[type='checkbox']").forEach((x) => {
                x.checked = false;
            });
            // Recalcula os textos dos gatilhos dos enum-selects.
            updateEnumSelectLabels();
            // Recalcula estado visual de campos flutuantes após limpeza.
            syncFloatingFields();

            // Reaplica filtros (agora limpos) para restaurar listagem completa.
            applyFilters();
        });

        // Sincroniza estado inicial dos campos flutuantes.
        syncFloatingFields();
    }

    // Conecta eventos de abertura e seleção da unidade de idade.
    function wireAgeUnitSelect() {
        // Obtém elemento raiz do seletor customizado.
        const select = document.getElementById("ageUnitSelect");
        // Obtém botão gatilho do seletor.
        const trigger = document.getElementById("ageUnitTrigger");
        // Coleta opções disponíveis de unidade.
        const options = Array.from(select.querySelectorAll(".age-unit-option"));

        // Alterna abertura do menu ao clicar no gatilho.
        trigger.addEventListener("click", (event) => {
            // Impede propagação para não disparar fechamento global.
            event.stopPropagation();
            // Lê estado atual do menu.
            const isOpen = select.classList.contains("open");
            // Aplica estado invertido de abertura.
            select.classList.toggle("open", !isOpen);
            // Atualiza atributo de acessibilidade com novo estado.
            trigger.setAttribute("aria-expanded", (!isOpen).toString());
        });

        // Registra comportamento de seleção para cada opção.
        options.forEach((option) => {
            // Ao clicar em uma opção, troca unidade e atualiza tela.
            option.addEventListener("click", () => {
                // Atualiza unidade corrente com dado da opção clicada.
                currentAgeUnit = option.dataset.unit;
                // Remove destaque de todas as opções.
                options.forEach((item) => item.classList.remove("active"));
                // Marca opção atual como ativa.
                option.classList.add("active");
                // Fecha menu após seleção.
                select.classList.remove("open");
                // Atualiza acessibilidade para estado fechado.
                trigger.setAttribute("aria-expanded", "false");
                // Reescreve rótulo de unidade exibido na interface.
                updateAgeUnitLabel();
                // Reaplica filtros usando nova unidade.
                applyFilters();
            });
        });

        // Fecha menu de unidade ao clicar em qualquer ponto externo.
        document.addEventListener("click", () => {
            // Remove estado aberto do seletor.
            select.classList.remove("open");
            // Atualiza atributo ARIA para refletir fechamento.
            trigger.setAttribute("aria-expanded", "false");
        });

        // Ajusta texto inicial da unidade ao carregar página.
        updateAgeUnitLabel();
    }

    // Atualiza texto do trigger do enum-select conforme quantidade selecionada.
    function updateEnumSelectLabel(selectElement) {
        // Obtém elemento de gatilho do select.
        const trigger = selectElement.querySelector(".enum-select-trigger");
        // Conta itens marcados dentro do select.
        const checked = selectElement.querySelectorAll("input[type='checkbox']:checked").length;

        // Caso nenhum item esteja marcado, limpa texto do trigger.
        if (checked === 0) {
            trigger.textContent = "";
            // Atualiza estado visual do campo como não preenchido.
            syncEnumFloatingField(selectElement);
            return;
        }

        // Escreve resumo singular/plural conforme quantidade marcada.
        trigger.textContent = checked === 1 ? "1 selecionado" : `${checked} selecionados`;
        // Atualiza estado visual do campo como preenchido.
        syncEnumFloatingField(selectElement);
    }

    // Atualiza o rótulo de todos os enum-selects presentes na página.
    function updateEnumSelectLabels() {
        // Percorre todos os selects customizados e sincroniza cada um.
        document.querySelectorAll(".enum-select").forEach(updateEnumSelectLabel);
    }

    // Conecta interações dos selects customizados de múltipla seleção.
    function wireEnumSelects() {
        // Percorre todos os enum-selects existentes na tela.
        document.querySelectorAll(".enum-select").forEach((selectElement) => {
            // Obtém gatilho de abertura do select atual.
            const trigger = selectElement.querySelector(".enum-select-trigger");

            // Impede que clique interno feche menu por propagação.
            selectElement.addEventListener("click", (event) => {
                event.stopPropagation();
            });

            // Controla abertura/fechamento ao clicar no gatilho.
            trigger.addEventListener("click", (event) => {
                // Impede propagação para listener global de documento.
                event.stopPropagation();
                // Verifica estado atual de abertura do select.
                const isOpen = selectElement.classList.contains("open");

                // Fecha outros selects que possam estar abertos.
                document.querySelectorAll(".enum-select.open").forEach((item) => {
                    item.classList.remove("open");
                    item.querySelector(".enum-select-trigger").setAttribute("aria-expanded", "false");
                });

                // Se select atual estava fechado, abre-o.
                if (!isOpen) {
                    selectElement.classList.add("open");
                    trigger.setAttribute("aria-expanded", "true");
                }
            });

            // Registra reação de atualização quando checkbox muda.
            selectElement.querySelectorAll("input[type='checkbox']").forEach((checkbox) => {
                checkbox.addEventListener("change", () => updateEnumSelectLabel(selectElement));
            });

            // Sincroniza rótulo inicial do select atual.
            updateEnumSelectLabel(selectElement);
        });

        // Fecha todos os selects quando houver clique fora deles.
        document.addEventListener("click", () => {
            // Percorre selects abertos para fechá-los.
            document.querySelectorAll(".enum-select.open").forEach((selectElement) => {
                selectElement.classList.remove("open");
                selectElement.querySelector(".enum-select-trigger").setAttribute("aria-expanded", "false");
            });
        });
    }

    // Busca dados da API e inicializa a primeira renderização da tabela.
    async function load() {
        // Requisita lista de bovinos ao backend.
        const res = await fetch("/api/bovines");
        // Converte payload para JSON e armazena em memória.
        allBovines = await res.json();
        // Exibe lista completa antes de aplicar filtros do usuário.
        renderRows(allBovines);
    }

    // Configura listeners e comportamentos de filtro ao iniciar página.
    wireFilters();
    // Dispara carregamento inicial dos dados.
    load();
})();
