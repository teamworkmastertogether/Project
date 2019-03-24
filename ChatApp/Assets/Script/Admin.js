$(document).ready(function () {
    $('.btn-create-user').on('click', function () {
        let check = true;
        $('.input2').removeClass('border-red');
        $('.input2').each(function () {
            if ($(this).val() === "" || $(this).val().length < 5 || $(this).val().length > 200) {
                $(this).addClass('border-red');
                check = false;
                $(this).tooltip("show");
            }
        });
        if (!isStudentEmail($("#userEmail").val())) {
            $('#userEmail[data-toggle="tooltip"]').tooltip("show");
            check = false;
        }
        if (check) {
            var dataObj = {
                Name: $('#add-user').find('#userName').val(),
                Email: $('#add-user').find('#userEmail').val()
            };
            console.log(dataObj);
            $('#add-user').find('#userName').val('');
            $('#add-user').find('#userEmail').val('');
            $.ajax({
                type: "POST",
                url: "/Admin/CreateUser",
                data: JSON.stringify(dataObj),
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                cache: false,
                success: function (result) {
                    $('#myModal').modal('hide');
                    Swal.fire(
                        'Thành công!',
                        'Bạn đã thêm thành viên thành công!',
                        'success'
                    );
                }, error: function (message) {
                    alert(message.responseText);
                }
            });
        } 
    });

    $('#userName').blur(function () {
        $(this).val(ChuanhoaTen($(this).val()));
    });

    function ChuanhoaTen(ten) {
        dname = ten;
        ss = dname.split(' ');
        dname = "";
        for (i = 0; i < ss.length; i++)
            if (ss[i].length > 0) {
                if (dname.length > 0) dname = dname + " ";
                dname = dname + ss[i].substring(0, 1).toUpperCase();
                dname = dname + ss[i].substring(1).toLowerCase();
            }
        return dname;
    }

    // Kiểm tra email
    function isStudentEmail(e) {
        var regex = /(^(\d{8}))((@gmail.com)$)/;
        if (regex.test(e)) return true;
        return false;
    }

    $('.input2').blur(function () {
        if ($(this).val() !== "") {
            $(this).removeClass('border-red');
        } else {
            $(this).addClass('border-red');
        }
    });
});