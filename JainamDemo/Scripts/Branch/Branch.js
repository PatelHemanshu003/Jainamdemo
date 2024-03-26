jQuery.noConflict();
jQuery(document).ready(function ($) {
    $('#InitDate').datepicker({
        dateFormat: 'mm/dd/yy',
        startDate: '-3d'
    });
    if ($('#hdnBranchType').val() != "")
    {
        
        $("#Branch_Type").val($('#hdnBranchType').val());
        $("#Branch_Db").val($('#hdnBranch_Db').val());

    }
    $('#Branch_Logo').change(function () {
        var input = this;
        if (input.files && input.files[0]) {
            for (var i = 0; i < input.files.length; i++) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    //$('#imageContainer').append('');
                    $('#imageContainer').attr('src', e.target.result)
                }
                reader.readAsDataURL(input.files[i]);
            }
        }
    });
    $('#Btnsave').click(function () {
        var formData = new FormData();
        formData.append('BranchCode', $("#Branch_Code").val());
        formData.append('BranchName', $("#Branch_Name").val());
        formData.append('BranchAddress', $("#Branch_Addr").val());
        formData.append('BranchType', $("#Branch_Type").val());
        formData.append('Active', $('#IsActive').is(':checked') ? "1" : "0");
        formData.append('BranchDb', $("#Branch_Db").val());
        formData.append('Branchdate', $('#InitDate').val());

        // Get file data
        var files = document.getElementById('Branch_Logo').files;
        if (files.length > 0) {
            // Append each file to FormData
            for (var i = 0; i < files.length; i++) {
                formData.append('files[]', files[i]);
            }
        }
        else {
            var srcValue = $('#imageContainer').attr('src');
            var modifiedSrc = srcValue.replace('/images/', '');
            formData.append('modifiedSrc', modifiedSrc);
        }
       

        $.ajax({
            url: "/Home/Save",
            type: 'POST',
            data: formData,
            contentType: false, 
            processData: false,
            success: function (result) {
                alert("Saved")
            },
        });
    });
});


