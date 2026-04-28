function loadVehicles() {

    let date = document.querySelector("input[name='date']").value;
    let start = document.querySelector("input[name='start']").value;
    let end = document.querySelector("input[name='end']").value;

    if (!date || !start || !end) return;

    fetch(`/Profile/GetFreeVehicles?date=${date}&start=${start}&end=${end}`)
        .then(res => res.json())
        .then(data => {

            let select = document.getElementById("vehicleSelect");
            select.innerHTML = "";

            data.forEach(v => {
                let option = document.createElement("option");
                option.value = v.id;
                option.text = `${v.brand} ${v.model} (${v.year})`;
                select.appendChild(option);
            });
        });
}

function openEdit(id, enrollmentId, lessonTypeId, date, start, end) {

    document.getElementById("editLessonForm").style.display = "block";

    document.getElementById("editId").value = id;
    document.getElementById("editEnrollmentId").value = enrollmentId;
    document.getElementById("editLessonTypeId").value = lessonTypeId;
    document.getElementById("editDate").value = date;
    document.getElementById("editStart").value = start;
    document.getElementById("editEnd").value = end;
}