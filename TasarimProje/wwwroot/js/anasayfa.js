$(function () {
    var ids = [];
    $(".malzemeBtn").on("click", function () {
        //var id = $(this).attr("data-id");
        var a = $('.Malzeme:checkbox:checked').attr("data-id");
        //_.xor(ids, [id]); // Diziye yeni id'yi ekle veya varsa kaldır


        //console.log(ids)
        //$.ajax(
        //    {
        //        type: "GET",
        //        url: '/Home/Index?ids=' + ids,
        //        success: function (data) {
        //            //console.log(data);
        //            //$(html).html(data);
        //            $("html").html(data);
        //        },
        //    }
        //);
    }); 

});

   
    //<script>
    //    $(document).ready(function () {
    //        $('input[type="checkbox"]').change(function () {
    //            var selectedIds = [];
    //            $('input[type="checkbox"]:checked').each(function () {
    //                selectedIds.push($(this).val());
    //            });

    //            $.ajax({
    //                url: '@Url.Action("FilterProjects", "YourController")',
    //                type: 'POST',
    //                data: { ids: selectedIds },
    //                traditional: true,
    //                success: function (result) {
    //                    $('#filteredProjects').html(result);
    //                }
    //            });
    //        });
    //    });
    //</script>

