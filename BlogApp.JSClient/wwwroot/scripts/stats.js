const body = document.getElementsByTagName("body")[0]; 

function loadIntoTable(data) {
    const headers = Object.keys(data[0]);
    const table = document.createElement("table");
    let tr = document.createElement("tr");
    for (let i = 0; i < headers.length; i++)
    {
        const th = document.createElement("th");
        th.textContent = headers[i];
        tr.appendChild(th);
    }
    table.appendChild(tr);
    for (let i = 0; i < data.length; i++) {
        tr = document.createElement("tr");
        for (let key in data[i]) {
            let td = document.createElement("td");
            td.textContent = data[i][key];
            tr.appendChild(td);
        }
        table.appendChild(tr);
    }
    table.style.display = 'none';
    body.append(table);
}

function showSelectedTable(value) {
    const tables = document.querySelectorAll('table');
    tables.forEach(t => t.style.display = 'none');
    tables[value].style.display = 'block';
}

window.onload = () => {
    const tableSelector = document.getElementById("table-selector");
    tableSelector.addEventListener("change", e => {
        e.preventDefault();
        showSelectedTable(e.target.value);
    });
    const statsRequests = [
        fetch("http://localhost:5828/Stats/GetAvgNumberOfComments"),
        fetch("http://localhost:5828/Stats/GetBlogRankingsByPopularity"),
        fetch("http://localhost:5828/Stats/GetMostPopularPostPerBlog"),
        fetch("http://localhost:5828/Stats/GetPostsCountPerCategory"),
        fetch("http://localhost:5828/Stats/GetAverageRatingOfPostsPerCategory")
    ];
    Promise.all(statsRequests)
        .then(responses => Promise.all(responses.map(r => r.json())))
        .then(results => Promise.all(results.map(r => loadIntoTable(r))))
        .then(_ => {
            document.getElementById("loading-text").style.display = 'none';
            document.getElementById("table-selector").style.display = 'block';
            showSelectedTable(tableSelector.value);
        });
}