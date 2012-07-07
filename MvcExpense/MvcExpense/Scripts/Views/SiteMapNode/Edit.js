
var parentIdSelectId = '#ParentId';
var previousSiblingSelectId = '#PreviousSibling';
var previousSiblingDivId = '#DivPreviousSibling';

$(document).ready(function () {
    $(previousSiblingDivId).hide();
    
    populatePreviousSiblingSelectControlWithSelectedParentId();

    $(parentIdSelectId).change(function () {
        populatePreviousSiblingSelectControlWithSelectedParentId();
    });

    var hasPreviousSibling = !(typeof previousSibling === 'undefined');

    if (hasPreviousSibling) {
        $(previousSiblingSelectId).val(previousSibling);
    }
});

function populatePreviousSiblingSelectControlWithSelectedParentId() {
    var parentId = $(parentIdSelectId).val();

    if (parentId != '') {
        populatePreviousSiblingSelectControl(parentId);
    }
}

function populatePreviousSiblingSelectControl(parentId) {
    var select = $(previousSiblingSelectId);

    select.empty();
    select.append($("<option>").val('').text('[ First ]'));

    var subtree = $jit.json.getSubtree(tree, parentId);
    var i = 0;
    $jit.json.eachLevel(subtree, 0, 1, function (node, depth) {
        if (depth == 0) {
            return;
        }
        var text = (i + 1) + ". " + node.data.title;
        select.append($("<option>").val(node.id).text(text));
        i++;
    });

    var previousSiblingDiv = $(previousSiblingDivId);
    var optionCount = select.find('option').length;

    if (optionCount > 1) {
        previousSiblingDiv.show(1000);
    } else {
        previousSiblingDiv.hide(1000);
    }
}
