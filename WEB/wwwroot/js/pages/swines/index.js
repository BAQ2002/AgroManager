(async function () {
    const grid = window.DataGrid.create({
        rowsSelector: "#rows",
        countSelector: "#count",
        pageSizeSelector: "#swinesPageSize",
        paginationInfoSelector: "#swinesPageInfo",
        prevPageSelector: "#swinesPrevPage",
        nextPageSelector: "#swinesNextPage",
        data: [],
        rowTemplate: (x) => `
      <tr>
        <td>${x.name ?? ""}</td>
        <td>${x.gender ?? ""}</td>
        <td>${x.origin ?? ""}</td>
        <td>${window.DataGrid?.formatDateBr(x.birthDate) ?? (x.birthDate ?? "")}</td>
        <td>${x.age ?? ""}</td>
        <td>${x.porcType ?? ""}</td>
      </tr>
    `
    });

    async function load() {
        const res = await fetch("/api/swines");
        const data = await res.json();
        grid.setData(data);
    }

    await load();
})();
