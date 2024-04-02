let connection = null;
const body = document.getElementsByTagName("body")[0];
let blogs = [];
let selectedBlog = null;
getdata();
setupSignalR();

function setupSignalR() {
    connection = new signalR.HubConnectionBuilder()
        .withUrl("http://localhost:5828/hub")
        .configureLogging(signalR.LogLevel.Information)
        .build();

    connection.on("BlogCreated", (user, message) => {
        getdata();
    });

    connection.on("BlogDeleted", (user, message) => {
        getdata();
    });

    connection.on("BlogUpdated", (user, message) => {
        getdata();
    });

    connection.onclose(async () => {
        await start();
    });
    start();
}

async function start() {
    try {
        await connection.start();
        console.log("SignalR Connected.");
    } catch (err) {
        console.log(err);
        setTimeout(start, 5000);
    }
};

async function getdata() {
    await fetch('http://localhost:5828/blog')
        .then(x => x.json())
        .then(y => {
            blogs = y;
            loadIntoTable(blogs);
        });
}

function loadIntoTable(data) {
    const headers = ["id", "blogName", "actions"];
    const table = document.getElementById("blog-data");
    table.innerHTML = "";
    let tr = document.createElement("tr");
    for (let i = 0; i < headers.length; i++) {
        const th = document.createElement("th");
        th.textContent = headers[i];
        tr.appendChild(th);
    }
    table.appendChild(tr);
    for (let i = 0; i < data.length; i++) {
        tr = document.createElement("tr");
        for (let key of headers) {
            let td = document.createElement("td");
            if (key != "actions") {
                td.textContent = data[i][key];
            }
            else {
                td.innerHTML = `<button type="button" class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#updateModal" onclick="fillForm(${data[i].id})">Update</button>`
                td.innerHTML += `<button type="button" class="btn btn-danger" onclick="remove(${data[i].id})">Delete</button>`
            }
            tr.appendChild(td);
        }
        table.appendChild(tr);
    }
    body.append(table);
}

function fillForm(id) {
    selectedBlog = blogs.find(b => b['id'] == id);
    document.getElementById("blogName").value = selectedBlog["blogName"];
}

function remove(id) {
    fetch('http://localhost:5828/blog/' + id, {
        method: 'DELETE',
        headers: { 'Content-Type': 'application/json', },
        body: null
    })
        .then(response => response)
        .then(data => {
            console.log('Success:', data);
            getdata();
        })
        .catch((error) => { console.error('Error:', error); });
}

function update() {
    let name = document.getElementById('blogName').value;
    fetch('http://localhost:5828/blog', {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json', },
        body: JSON.stringify(
            {
                id: selectedBlog.id,
                blogName: name
            })
    })
        .then(response => response)
        .then(data => {
            console.log('Success:', data);
            selectedBlog = null;
            getdata();
        })
        .catch((error) => { console.error('Error:', error); });
}

function create() {
    let name = document.getElementById('newBlogName').value;
    fetch('http://localhost:5828/blog', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json', },
        body: JSON.stringify(
            {
                blogName: name
            })
    })
        .then(response => response)
        .then(data => {
            console.log('Success:', data);
            getdata();
        })
        .catch((error) => { console.error('Error:', error); });
}

document.getElementById("update-button").addEventListener("click", e => {
    e.preventDefault();
    update();
});

document.getElementById("create-button").addEventListener("click", e => {
    e.preventDefault();
    create();
});