///The buttons of paging will post to server
function doPagingPost(triggerEl, hidElementId) {
    if (triggerEl == null) {
        return false;
    }
    var pageIndex = 1;
    var eltype = triggerEl.tagName.toLocaleLowerCase();
    if (eltype == "select" || (eltype == "input" && jQuery(triggerEl).attr("type").toLocaleLowerCase() == "text")) {
        pageIndex = jQuery(triggerEl).val();
    } else {
        pageIndex = jQuery(triggerEl).attr("page");
    }
    jQuery("#" + hidElementId + "").val(pageIndex);
    var $form = jQuery("#" + hidElementId + "").closest("form");
    if ($form.length == 0)
    {
        return false;
    }
    $form.submit();
}
//異步提交查詢：改變PageSize觸發,2013-9-3,zjj
function doPagingPostByChangePageSize(triggerEl, hidElementId) {
    var pageSize = jQuery(triggerEl).val()
    jQuery("#" + hidElementId + "").val(pageSize);
    jQuery('.ajaxForm').submit();
}