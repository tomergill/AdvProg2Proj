function checkGameDetails(nameEle, rowsEle, colsEle, err) {

    if (nameEle === null || rowsEle === null || colsEle === null || err === null)
        return null;

    err.style.setProperty("visibility", "hidden");

    var name = nameEle.value;
    var rows = parseInt(rowsEle.value);
    var cols = parseInt(colsEle.value);

    if (isNaN(rows)) {
        err.innerHTML = "<strong>Error! rows number was empty. rows must be between " + parseInt(rowsEle.getAttribute("min")).toString() + " to " + parseInt(rowsEle.getAttribute("max")).toString() + ".</strong>";
        err.style.setProperty("visibility", "visible");
        rowsEle.setAttribute("value", (rowsEle.getAttribute("min")).toString());
        return false;
    }
    if (isNaN(cols)) {
        err.innerHTML = "<strong>Error! cols number was empty. cols must be between " + parseInt(colsEle.getAttribute("min")).toString() + " to " + parseInt(colsEle.getAttribute("max")).toString() + ".</strong>";
        err.style.setProperty("visibility", "visible");
        colsEle.setAttribute("value", (colsEle.getAttribute("min")).toString());
        return false;
    }
    if (name == "")
    {
        err.innerHTML = "<strong>Error! maze must have a name.</strong>"
        err.style.setProperty("visibility", "visible");
        return false;
    }

    if (!checkIfInputNumberValid(rowsEle, err, "rows", rows) || !checkIfInputNumberValid(colsEle, err, "cols", cols))
        return false;

    return true;
}

function checkIfInputNumberValid(inputEle, errEle, name, value) {
    var min = parseInt(inputEle.getAttribute("min"));
    var max = parseInt(inputEle.getAttribute("max"));

    if (isNaN(min) || isNaN(max))
        return false;

    if (max < min) //switch them - someone messed up the HTML >:(
    {
        inputEle.setAttribute("max", min.toString());
        inputEle.setAttribute("min", max.toString());
        var temp = min;
        min = max;
        max = min;
    }

    if (value == '.')
        inputEle.setAttribute("value", min.toString());

    value = parseInt(value);

    if (isNaN(value)) {
        inputEle.setAttribute("value", (Math.abs(max - min) / 2).toString());
        err.innerHTML = "<strong>Error! " + name + " number was illegal. " + name + " must be between " + parseInt(inputEle.getAttribute("min")).toString() + " to " + parseInt(inputEle.getAttribute("max")).toString() + ".</strong>";
        errEle.style.setProperty("visibility", "visible");
        return false;
    }
    if (value > max) {
        inputEle.setAttribute("value", max.toString());
        err.innerHTML = "<strong>Error! " + name + " number was bigger than permitted. " + name + " must be between " + parseInt(inputEle.getAttribute("min")).toString() + " to " + parseInt(inputEle.getAttribute("max")).toString() + ".</strong>";
        errEle.style.setProperty("visibility", "visible");
        return false;
    }
    if (value < min) {
        inputEle.setAttribute("value", min.toString());
        err.innerHTML = "<strong>Error! " + name + " number was smaller than permitted. " + name + " must be between " + parseInt(inputEle.getAttribute("min")).toString() + " to " + parseInt(inputEle.getAttribute("max")).toString() + ".</strong>";
        errEle.style.setProperty("visibility", "visible");
        return false;
    }

    return true; //everything okay
}