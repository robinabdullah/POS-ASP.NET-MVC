var Models = []

function LoadModels(type) {
    
        $.ajax({
            type: "GET",
            url: 'Purchase/Fill_DDL_Model',
            data: {'ID' : $(type).val()},
            success: function (data) {
                renderModels($(type).parents('.mycontainer').find('select.model'),data);
            },
            error: function(error){
                console.log(error);
            }
        })
    
    
}
function renderModels(element) {
    var $ele = $(element);
    $ele.empty();
    $ele.append($(''))
}
//function renderModels(element) {
//    var $ele = $(element);
//    $ele.empty();
//    $ele.append($('<option/>').val('0').text('select'));
//    $.each(Models, function(i, val){
//        $ele.append($('<option/>').val(val.ID).text(val.Model));
//    })
//}