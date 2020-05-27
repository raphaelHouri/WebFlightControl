
        // ************************ Drag and drop ***************** //
let dropArea = document.getElementById("drop-visible-area")

    // Prevent default drag behaviors
            ;['dragenter', 'dragover', 'dragleave', 'drop'].forEach(eventName => {
        dropArea.addEventListener(eventName, preventDefaults, false)
                document.body.addEventListener(eventName, preventDefaults, false)
            })

            // Highlight drop area when item is dragged over it
            ;['dragenter', 'dragover'].forEach(eventName => {
        dropArea.addEventListener(eventName, highlight, false)
    })

            ;['dragleave', 'drop'].forEach(eventName => {
        dropArea.addEventListener(eventName, unhighlight, false)
    })

        // Handle dropped files
        dropArea.addEventListener('drop', handleDrop, false)

        function preventDefaults(e) {
        e.preventDefault()
            e.stopPropagation()
        }

        function highlight(e) {
            let output1 = 
                `                    <div id="drop-area"><div id="dragText"><form class="my-form">
                                <p>Upload multiple files with the file dialog or by dragging and dropping images onto the dashed region</p>
                                <input type="file" id="fileElem" multiple accept="image/*" onchange="handleFiles(this.files)">
                                <label class="button" for="fileElem">Select some files</label>
                            </form>
                            <progress id="progress-bar" max=100 value=0></progress>
                            <div id="gallery" /><div/><div/>`
                ;

            document.getElementById('addOutput').innerHTML = output1;
            //dropArea.classList.add('highlight')

}

        function unhighlight(e) {
        
            //dropArea.classList.remove('highlight')
            let myobj = document.getElementById("drop-area");
            myobj.remove();
    }

        function handleDrop(e) {
            var dt = e.dataTransfer
            var file = dt.files[0];
            handleFiles(file)
        }
        function handleFiles(file) {
        readFile(file)
    }

        function preparePost(file) {
        //from obect to string

        let todoAsStr = JSON.stringify(file);
            return {
        "method": "POST",
                "headers": {'Content-Type': 'application/json' },
                "body": todoAsStr
            }
        }

        function  readFile(file, i) {
        let reader = new FileReader()
            reader.readAsText(file);
            let res;
            reader.onload = function () {
        //console.log(reader.result);
        res = JSON.parse(reader.result);
                uploadFile(res);
            };

            reader.onerror = function () {
        console.log(reader.error);
            };

        }

        async function uploadFile(file) {
        let postOptions = preparePost(file)
            console.log(postOptions);
            await fetch("api/FlightPlan", postOptions)
                .then(response => response.json())
                .then(appendItem)
                .catch(error => console.log(error))
        }

        function appendItem(res) {
        /*  let todos = document.getElementById("t_list");
          let todoEl = document.createElement("li");
          todoEl.id = todo.id;
          todoEl.innerHTML = `${todo.name}`;
          todo.append(todoEl);*/
        console.log(res);
        }
        function hide() {
        let x = document.getElementById("drop-area");
            x.style.visibility === "hidden";
        }

        function show() {
        let x = document.getElementById("drop-area");
            x.style.visibility === "inherit;";
        }
