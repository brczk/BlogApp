let connection = null;
const body = document.getElementsByTagName("body")[0];
let comments = [];
let selectedComment = null;
getdata();
setupSignalR();

function setupSignalR() {
    connection = new signalR.HubConnectionBuilder()
        .withUrl("http://localhost:5828/hub")
        .configureLogging(signalR.LogLevel.Information)
        .build();

    connection.on("CommentCreated", (user, message) => {
        getdata();
    });

    connection.on("CommentDeleted", (user, message) => {
        getdata();
    });

    connection.on("CommentUpdated", (user, message) => {
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
    await fetch('http://localhost:5828/comment')
        .then(x => x.json())
        .then(y => {
            comments = y;
            loadIntoTable(comments);
        });
}

function loadIntoTable(data) {
    const headers = ["postRating", "userName","content", "actions"];
    const table = document.getElementById("comment-data");
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
    selectedComment = comments.find(c => c['id'] == id);
    document.getElementById("commentContent").value = selectedComment["content"];
    document.getElementById("postRating").value = selectedComment["postRating"];
    document.getElementById("commentUserName").value = selectedComment["userName"];
    document.getElementById("postId").value = selectedComment["postId"];
}

function remove(id) {
    fetch('http://localhost:5828/comment/' + id, {
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
    let commentContent = document.getElementById('commentContent').value;
    let postRating = document.getElementById('postRating').value;
    let commentUserName = document.getElementById('commentUserName').value;
    let postId = document.getElementById('postId').value;


    fetch('http://localhost:5828/comment', {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json', },
        body: JSON.stringify(
            {
                id: selectedComment.id,
                userName: commentUserName,
                postRating: postRating,
                content: commentContent,
                postId: postId
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
    let postId = document.getElementById('newPostId').value;
    let postRating = document.getElementById('newPostRating').value;
    let userName = document.getElementById('newCommentUserName').value;
    let content = document.getElementById('newCommentContent').value;

    fetch('http://localhost:5828/comment', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json', },
        body: JSON.stringify(
            {
                postId: postId,
                userName: userName,
                content: content,
                postRating: postRating
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