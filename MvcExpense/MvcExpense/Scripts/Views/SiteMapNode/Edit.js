
var parentIdSelectId = '#ParentId';
var previousSiblingSelectId = '#PreviousSibling';
var divPreviousSibling = '#DivPreviousSibling';

$(document).ready(function () {
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
    populatePreviousSiblingSelectControl(parentId);
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
    
    if (i > 1) {
        $(divPreviousSibling).show(1000);
    } else {
        $(divPreviousSibling).hide(1000);
    }
}
