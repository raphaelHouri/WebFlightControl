﻿// ************************ Drag and drop ***************** //
let dropArea = document.getElementById("drop-visible-area");

// Prevent default drag behaviors
["dragenter", "dragover", "dragleave", "drop"].forEach(eventName => {
    dropArea.addEventListener(eventName, preventDefaults, false);
    document.body.addEventListener(eventName, preventDefaults, false);
});

// Highlight drop area when item is dragged over it
["dragenter", "dragover"].forEach(eventName => {
    dropArea.addEventListener(eventName, highlight, false);
});
["dragleave", "drop"].forEach(eventName => {
    dropArea.addEventListener(eventName, unhighlight, false);
});

// Handle dropped files
dropArea.addEventListener("drop", handleDrop, false);

function preventDefaults(e) {
    e.preventDefault();
    e.stopPropagation();
}
//when we drag to the place this elements will appear
function highlight() {
    let output1 = `             
        <div id="drop-area"><div id="dragText"><form class="my-form">
        <p>Upload multiple files with the file dialog or by dragging and dropping
        images onto the dashed region</p>
        <input type="file" id="fileElem" multiple accept="image" 
        onchange="handleFiles(this.files)">
        <label class="button" for="fileElem">Select some files</label>
    </form>
    <progress id="progress-bar" max=100 value=0></progress>
    <div id="gallery" /><div/><div/>`;
    document.getElementById("addOutput").innerHTML = output1;
}
//when we drop it already this elements will disappear
function unhighlight() {
    let myobj = document.getElementById("drop-area");
    myobj.remove();
}

function handleDrop(e) {
    let dt = e.dataTransfer;
    let file = dt.files[0];
    handleFiles(file);
}
function handleFiles(file) {
    readFile(file);
}

function preparePost(file) {
    let todoAsStr = JSON.stringify(file);
    return {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: todoAsStr
    };
}
//read the JSON file
function readFile(file) {
    let reader = new FileReader();
    reader.readAsText(file);
    let res;
    reader.onload = function () {
        res = JSON.parse(reader.result);
        uploadFile(res);
    };

    reader.onerror = function () {
        alert(reader.error);
    };
}

//upload JSON file to database
async function uploadFile(file) {
    let postOptions = preparePost(file);
    console.log(postOptions);
    let response = await fetch("api/FlightPlan", postOptions);

    if (response.status == 201) {
        await response.json();
        appendItem();
        //alert("New flight plan added to the data base");
    } else {
        try {
            throw new Error(response.status);
        } catch (error) {
            alert(error);
        }
    }
}
//call to func to update the screen display
function appendItem() {
    getAllFlight();
}
//hide when we didn"t drag file
function hide() {
    let x = document.getElementById("drop-area");
    x.style.visibility === "hidden";
}
//show when we are draging file

function show() {
    let x = document.getElementById("drop-area");
    x.style.visibility === "inherit;";
}
