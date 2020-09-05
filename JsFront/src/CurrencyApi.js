const url = "https://localhost:5001/currencyApi"

function sendRequestPost(method, url, body = null){
    const headers = {
        'Content-Type': 'application/json'
    }

    return fetch(url, {
        method: method,
        body: body,
        headers: headers
    }).then(response => {
        return response.json()
    })
}



function sendRequestGet(method, url){
    return fetch(url).then(response => {
        return response.json()
    })
}



const h = document.getElementById('nameCurrency')


sendRequestGet('GET', url).then(data => {
    h.innerText = data.name + ' на ' + data.date
    addRow('currencyTable', data)
    console.log(data)
})

function addRow(id, date){
    let tbody = document.getElementById(id).getElementsByTagName("tbody")[0];

    for (let i = 0; i < date.currencies.length; i++) {

        let row = document.createElement("tr")

        let td1 = document.createElement("td")
        let td2 = document.createElement("td")
        td1.appendChild(document.createTextNode(date.currencies[i].isoCode))
        td2.appendChild (document.createTextNode(date.currencies[i].value))
        row.appendChild(td1);
        row.appendChild(td2);
        tbody.appendChild(row);
    }
}

