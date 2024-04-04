let connection = null;
const body = document.getElementsByTagName("body")[0];
let posts = [];
let selectedPost = null;
getdata();
setupSignalR();

function setupSignalR() {
    connection = new signalR.HubConnectionBuilder()
        .withUrl("http://localhost:5828/hub")
        .configureLogging(signalR.LogLevel.Information)
        .build();

    connection.on("postCreated", (user, message) => {
        getdata();
    });

    connection.on("postDeleted", (user, message) => {
        getdata();
    });

    connection.on("postUpdated", (user, message) => {
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
    await fetch('http://localhost:5828/post')
        .then(x => x.json())
        .then(y => {
            posts = y;
            loadIntoTable(posts);
        });
}

function loadIntoTable(data) {
    const headers = ["category", "postAuthor", "content", "actions"];
    const table = document.getElementById("post-data");
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
    selectedPost = posts.find(p => p['id'] == id);
    document.getElementById("blogId").value = selectedPost["blogId"];
    document.getElementById("postAuthor").value = selectedPost["postAuthor"];
    document.getElementById("postCategory").value = selectedPost["category"];
    document.getElementById("postContent").value = selectedPost["content"];
}

function remove(id) {
    fetch('http://localhost:5828/post/' + id, {
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
    let postContent = document.getElementById('postContent').value;
    let postCategory = document.getElementById('postCategory').value;
    let postAuthor = document.getElementById('postAuthor').value;
    let blogId = document.getElementById('blogId').value;
    fetch('http://localhost:5828/post', {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json', },
        body: JSON.stringify(
            {
                id: selectedPost.id,
                postAuthor: postAuthor,
                category: postCategory,
                content: postContent,
                blogId: blogId
            })
    })
        .then(response => response)
        .then(data => {
            console.log('Success:', data);
            getdata();
        })
        .catch((error) => { console.error('Error:', error); });
}

function create() {
    let blogId = document.getElementById('newBlogId').value;
    let postAuthor = document.getElementById('newPostAuthor').value;
    let postCategory = document.getElementById('newPostCategory').value;
    let postContent = document.getElementById('newPostContent').value;

    fetch('http://localhost:5828/post', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json', },
        body: JSON.stringify(
            {
                blogId: blogId,
                postAuthor: postAuthor,
                category: postCategory,
                content: postContent
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