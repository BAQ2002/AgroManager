(async function () {
    // Declara função assíncrona responsável por buscar dados e renderizar a tabela.
    async function load() {
        // Faz requisição HTTP para o endpoint de suínos.
        const res = await fetch("/api/swines");
        // Converte a resposta HTTP para JSON.
        const data = await res.json();

        // Atualiza o conteúdo HTML do corpo da tabela com as linhas geradas a partir dos dados.
        document.getElementById("rows").innerHTML = data.map((x) => `
      <tr>
        <td>${x.name ?? ""}</td>
        <td>${x.gender ?? ""}</td>
        <td>${x.origin ?? ""}</td>
        <td>${x.birthDate ?? ""}</td>
        <td>${x.age ?? ""}</td>
        <td>${x.porcType ?? ""}</td>
      </tr>
    `).join("");

        // Atualiza o texto do contador com a quantidade de registros exibidos.
        document.getElementById("count").innerText = `${data.length} registro(s) exibidos`;
    }

    // Executa a função de carregamento inicial da página.
    await load();
})();
