document.addEventListener("DOMContentLoaded", function () {

    const roleSelect = document.getElementById("roleSelect");
    const courseBlock = document.getElementById("courseBlock");

    if (!roleSelect || !courseBlock) return;

    function updateUI() {
        const roleId = roleSelect.value;

        if (roleId === "3") {
            courseBlock.style.display = "block";
        } else {
            courseBlock.style.display = "none";
        }
    }

    roleSelect.addEventListener("change", updateUI);

    updateUI();
});