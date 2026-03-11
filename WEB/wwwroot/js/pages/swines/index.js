(async function () {
    async function load() {
        const res = await fetch("/api/swines");
        const data = await res.json();

        document.getElementById("rows").innerHTML = data.map(x => `
      <tr>
        <td>${x.name ?? ""}</td>
        <td>${x.gender ?? ""}</td>
        <td>${x.origin ?? ""}</td>
        <td>${x.birthDate ?? ""}</td>
        <td>${x.age ?? ""}</td>
        <td>${x.porcType ?? ""}</td>
      </tr>
    `).join("");

        document.getElementById("count").innerText = `${data.length} registro(s) exibidos`;
    }

    await load();
})();
