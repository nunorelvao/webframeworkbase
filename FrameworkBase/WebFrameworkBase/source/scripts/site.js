//Initializers for all site

$(function () {
    //enable Jquery Tootips
    togleJqueryTooltips(true);
});

function togleJqueryTooltips(enabled) {
    if (enabled === true) {
        $('[data-toggle="tooltip"]').tooltip();
    }
}

var maxRetrysTocloseWaintingModal = 10;
var retrysTocloseWaintingModal = 0;
function showWaitingScreen(enabled) {
    var modalIsVisible = $("#waitModal").is(":visible");
    if (enabled === true & modalIsVisible === false) {
        //force count of retrys to 0 again
        retrysTocloseWaintingModal = 0;
        $("#waitModal").modal('show');
    }
    else if (enabled === false) {
        //if modal is already visible force close
        if (modalIsVisible) {
            $("#waitModal").modal('hide');
        }
        else {
            //It was asked to be closed but its not yet opened
            //retrigger the close method in intervals of 50 ms
            if (retrysTocloseWaintingModal <= maxRetrysTocloseWaintingModal) {
                setInterval(function () { showWaitingScreen(false); }, 50);
                retrysTocloseWaintingModal++;
            }
        }


    }
}

function activateDatePicker() {

    $(function () {
        $(".datepicker").datepicker(
            {
                changeMonth: true,
                changeYear: true,
                dateFormat: 'dd/mm/yy',
                minDate: '01/01/1930',
                yearRange: "1930:+00"
            });
    });
}

function activateColorPicker() {

    $(function () {
        // Basic instantiation:
        $('#usercolorbg-pick, #usercolorbg-minpick').colorpicker();


        // Example using an event, to change the color of the .jumbotron background:
        $('#usercolorbg-pick, #usercolorbg-minpick').on('colorpickerChange', function (event) {
            $('body').css('background-color', event.color.toString());
        });
    });
}




//######## UI Workable Framework for permissions ########

//var prototypeListOfControlsPermissionsAllInPage = new Array();
var prototypeListOfControlsPermissionsEnabled = new Array();
var prototypeListOfControlsPermissionsVisible = new Array();

var prototypeListOfControlsPermissionsEnabledRemove = new Array();
var prototypeListOfControlsPermissionsVisibleRemove  = new Array();

//Highlight controls and set as edit mode
function highlightControlsToEdit(e) {

    var isEdit = $(e).attr("data-isedit");

    var elementsSelectorFilter = "input:not(:hidden), span:not(:hidden), button, select, a, label";

    if (isEdit === "false") {

        $(e).attr("data-isedit", "true");
        $(e).css("color", "yellow");
        $(elementsSelectorFilter).each(function () {

            ////add all controls
            //prototypeListOfControlsPermissionsAllInPage.pushIfNotExist($(this), function (e) {
            //    return $(e).attr("id") === $(control).attr("id");
            //});

            var attrUniqueLeft = "control_";
            var attrUniqueMiddle = $("title").html().hashCode(); // $("body").find("h2").first().text();
            var attrUniqueRight = "_" + $(this).attr('id');
            var elemId = $(this).attr('id');
            var atttUniqueControlID = attrUniqueLeft + attrUniqueMiddle + attrUniqueRight;

            if (atttUniqueControlID.indexOf('undefined') >= 0) {
                $(this).css("border", "1px solid red");
                //in this case do not add control identity because has error
            }
            else {
                if ($(this) !== $(e)) {


                    var isVisible = true;
                    if ($(this).is(":hidden")) { isVisible = false; }
                    var classBadgeVisible = 'class="badge badge-sm bg-primary"';
                    if (isVisible === false) classBadgeVisible = 'class="badge badge-sm bg-danger"';


                    var isEnabled = true;
                    if ($(this).attr("disabled") !== undefined) { isEnabled = false; }
                    var classBadgeEnabled = 'class="badge badge-sm bg-primary"';
                    if (isEnabled === false) classBadgeEnabled = 'class="badge badge-sm bg-danger"';



                    $(this).css("border", "3px solid yellow");
                    $(this).attr("data-control-identify", atttUniqueControlID);
                    $(this).before('<div id="' + atttUniqueControlID + '" data-control-identify-control="yes" class="data-control-identify-control-block" data-control-isvisible="' + isVisible + '" data-control-isenabled="' + isEnabled + '"> ' +
                        '<span ' + classBadgeVisible + '  title="visible_' + $(this).attr("data-control-identify") + '" data-target-control-id=' + elemId + ' onclick="setControlsPermissionsVisibility($(this))" ">V</span>' +
                        '<span ' + classBadgeEnabled + '  title="enabled_' + $(this).attr("data-control-identify") + '" data-target-control-id=' + elemId + ' onclick="setControlsPermissionsEnabled($(this))" ">E</span>' +
                        '</div> ');
                }
            }

        });


    }
    else {
        $(e).attr("data-isedit", "false");
        $(e).css("color", "");
        $(elementsSelectorFilter).each(function () { $(this).css("border", "none"); });
        $("div[data-control-identify-control='yes']").remove();
    }




}

function setControlsPermissionsVisibility(e) {
    //alert(e.parent('div').attr('id') + " : VISIBLE: " + e.parent('div').first().attr('data-control-isvisible'));

    var isVisibleParent = e.parent('div').first().attr('data-control-isvisible');
    var targetElemId = $(e).attr("data-target-control-id");

    if (isVisibleParent === "true") {
        $(e).removeClass("bg-primary").addClass("bg-danger");
        $(e).parent('div').first().attr('data-control-isvisible', 'false');


        //Set prototypeListOfControlsPermissionsVisible
        var targetElemToAdd = $("#" + targetElemId).first();
        if (targetElemToAdd !== undefined) {
            setPermissions(targetElemToAdd, 'V');
        }
    }
    else {
        $(e).removeClass("bg-danger").addClass("bg-primary");
        $(e).parent('div').first().attr('data-control-isvisible', 'true');

        //Remove prototypeListOfControlsPermissionsVisible
        var targetElemToRemove = $("#" + targetElemId).first();
        if (targetElemToRemove !== undefined) {
            removePermissions(targetElemToRemove, 'V');
        }
    }


}


function setControlsPermissionsEnabled(e) {
    // alert(e.parent('div').attr('id') + " : ENABLED: " + e.parent('div').first().attr('data-control-isenabled'));

    var isEnabledParent = e.parent('div').first().attr('data-control-isenabled');
    var targetElemId = $(e).attr("data-target-control-id");

    if (isEnabledParent === "true") {
        $(e).removeClass("bg-primary").addClass("bg-danger");
        $(e).parent('div').first().attr('data-control-isenabled', 'false');

        
        //Set prototypeListOfControlsPermissionsEnabled 
        var targetElemToAdd = $("#" + targetElemId).first();
        if (targetElemToAdd !== undefined) {
            setPermissions(targetElemToAdd, 'E');
        }
        
    }
    else {
        $(e).removeClass("bg-danger").addClass("bg-primary");
        $(e).parent('div').first().attr('data-control-isenabled', 'true');

        //Remove prototypeListOfControlsPermissionsVisible
        var targetElemToRemove = $("#" + targetElemId).first();
        if (targetElemToRemove !== undefined) {
            removePermissions(targetElemToRemove, 'E');
        }
    }

}

//Extension for string use as = string.hashCode();
String.prototype.hashCode = function () {
    var hash = 0;
    if (this.length === 0) return hash;
    for (i = 0; i < this.length; i++) {
        char = this.charCodeAt(i);
        hash = ((hash << 5) - hash) + char;
        hash = hash & hash; // Convert to 32bit integer
    }
    return hash;
}


function setPermissions(control, typeofpermission) {

    if (typeofpermission === 'E') {

       // removePermissions(control, typeofpermission);

        prototypeListOfControlsPermissionsEnabled.pushIfNotExist(control, function (e) {
            return $(e).attr("id") === $(control).attr("id");
        });
    }
    else if (typeofpermission === 'V') {

        //removePermissions(control, typeofpermission);

        prototypeListOfControlsPermissionsVisible.pushIfNotExist(control, function (e) {
            return $(e).attr("id") === $(control).attr("id");
        });
    }
}

function removePermissions(control, typeofpermission) {


    if (typeofpermission === 'E') {

        // removePermissions(control, typeofpermission);

        prototypeListOfControlsPermissionsEnabledRemove.pushIfNotExist(control, function (e) {
            return $(e).attr("id") === $(control).attr("id");
        });
    }
    else if (typeofpermission === 'V') {

        //removePermissions(control, typeofpermission);

        prototypeListOfControlsPermissionsVisibleRemove.pushIfNotExist(control, function (e) {
            return $(e).attr("id") === $(control).attr("id");
        });
    }
}

function savePermisisonsInMemory() {
    
    //cycle List for Disabled to add
    for (var a = 0, lena = prototypeListOfControlsPermissionsEnabled.length; a < lena; a++) {
        console.log( "Adding to Enabled List: " + $(prototypeListOfControlsPermissionsEnabled[a]).attr("id"));
        $(prototypeListOfControlsPermissionsEnabled[a]).attr("disabled", "disabled");
    }

    //cycle List for Visible to add
    for (var b = 0, lenb = prototypeListOfControlsPermissionsVisible.length; b < lenb; b++) {
        console.log("Adding to Visibility List: " + $(prototypeListOfControlsPermissionsVisible[b]).attr("id"));
        $(prototypeListOfControlsPermissionsVisible[b]).hide();
    }


    //cycle List for Disabled to remove
    for (var c = 0, lenc = prototypeListOfControlsPermissionsEnabledRemove.length; c < lenc; c++) {
        console.log("Removing from Enabled List: " + $(prototypeListOfControlsPermissionsEnabledRemove[c]).attr("id"));
        $(prototypeListOfControlsPermissionsEnabledRemove[c]).removeAttr("disabled");
    }

    //cycle List for Visible to remove
    for (var d = 0, lend = prototypeListOfControlsPermissionsVisibleRemove.length; d < lend; d++) {
        console.log("Removing from Visibility List: " + $(prototypeListOfControlsPermissionsVisibleRemove[d]).attr("id"));
        $(prototypeListOfControlsPermissionsVisibleRemove[d]).show();
    }

    prototypeListOfControlsPermissionsEnabled = new Array();
    prototypeListOfControlsPermissionsVisible = new Array();
    prototypeListOfControlsPermissionsEnabledRemove = new Array();
    prototypeListOfControlsPermissionsVisibleRemove = new Array();

}

// check if an element exists in array using a comparer function
// comparer : function(currentElement)
Array.prototype.inArray = function (comparer) {
    for (var i = 0; i < this.length; i++) {
        if (comparer(this[i])) return true;
    }
    return false;
};

Array.prototype.inArrayIndex = function (comparer) {
    for (var i = 0; i < this.length; i++) {

        if ($(comparer).attr("id") === this[i].attr("id")) {
            return i;
        }
    }
    return -1;
};

// adds an element to the array if it does not already exist using a comparer 
// function
Array.prototype.pushIfNotExist = function (element, comparer) {
    if (!this.inArray(comparer)) {
        this.push(element);
    }
}; 




//######## UI Workable Framework for permissions ########