document.addEventListener("DOMContentLoaded", init, false);
var totFiles = 0;
var fdata = new FormData();

function init() {

    document.querySelector('#ajax_file').addEventListener('change', handleFileSelect, false);

    
}

function fileReset() {
    clearFileInput();
    $('#progress-container').empty();
    totFiles = 0;
    $("#per").text('0 kb');
}

function clearFileInput() {

    var oldInput = document.querySelector('#ajax_file');

    var newInput = document.createElement("input");

    newInput.type = "file";
    newInput.id = oldInput.id;
    newInput.name = oldInput.name;
    newInput.multiple = true;
    newInput.className = oldInput.className;
    newInput.style.cssText = oldInput.style.cssText;
    // copy any other relevant attributes

    oldInput.parentNode.replaceChild(newInput, oldInput);

    document.querySelector('#ajax_file').addEventListener('change', handleFileSelect, false);


}



function handleFileSelect(e) {

    if (!e.target.files) return;

    //$('#progress-container').empty();

    var files = e.target.files;
 
    for (var i = 0; i < files.length; i++) {

        //파일 포멧 형식 지정
        //var regEx = /^(image|video|audio)\//;
            
       
        totFiles = totFiles + files[i].size;


        var bar = '<li class="divProgress list-group-item" id="' + i + '" >' +
                    '<span class="abort" id=about-"' + i + '">&times;</span>' +
                    '<div id="divTitle-' + i + '">' + files[i].name + ' - ' + (files[i].size / 1024).toFixed(0).replace(/\B(?=(\d{3})+(?!\d))/g, ",") + 'kb </div>' +
                    '</li>';
                
        $("#progress-container").append(bar);

        fdata.append("files", files[i]);

        
    }

    
    $("#per").text((totFiles / 1024).toFixed(0).replace(/\B(?=(\d{3})+(?!\d))/g, ",") + 'kb');

    if (totFiles > 204800000) {
        alert("전체 저장 용량(200MB)을 초과 하였습니다.");
        clearFileInput();
        $('#progress-container').empty();
    }
}

//서버로직으로 저장 테스트
function btn_submit() {

    //if (document.querySelector('#ajax_file').files.length > 0) {

        ajaxUploadForm.submit();
    //}

}


$(document).ready(function () {

    //var fdata = new FormData();

    //$('#btnUpload').on('click', function () {


    //Ajax 저장 테스트
    $("#ajaxUploadForm").submit(function (e) {

        e.preventDefault();

        //유효성 검사 
        if (!$(this).valid()) return false;

        
        //abort(X)를 지움
        $(".abort").text('');

        

        //폼 데이터부분 Serialize로 대체
        //var fmdata = $form.serializeArray();

        //$.each(fmdata, function (key, input) {
        //    fdata.append(input.name, input.value);
        //});


        //폼의 파일 부분
        //var files = $('#ajax_file').prop("files");
        

        //for (var i = 0; i < files.length; i++) {
        //    fdata.append("files", files[i]);

        //}


        $.ajax({
            //url: "/Movies/Create",
            url: "/Movies/Create?handler=AjaxArticle",
            type: "Post",
            data: $('form').serialize(),
            
            //contentType: false,  //Serializ에는 사용 안함
            //processData: false,  //Serializ에는 사용 안함
            //data: fdata,  //폼데이터와 파일을 모두 가져올 때

            success: function (e) {

                //등록확인
                //alert(e);
                
                //fdata값 확인
                //alert(fdata.getAll("files").length);
                //for (var value of fdata.values()) {
                //    alert(value.name);
                //}

                
                if (fdata.getAll("files").length > 0) {

                    $.ajax({
                        type: "POST",
                        url: "/Movies/Create?handler=Upload&Id=" + e,

                        data: fdata,
                        contentType: false,
                        processData: false,

                        beforeSend: function (xhr) {
                            xhr.setRequestHeader("XSRF-TOKEN",
                                $('input:hidden[name="__RequestVerificationToken"]').val());
                        },
                        
                        xhr: function () {

                            var xhr = $.ajaxSettings.xhr();
                            if (xhr.upload) {

                                xhr.upload.addEventListener("progress", function (progress) {
                                    var total = Math.round((progress.loaded / progress.total) * 100);
                                    //텍스트로 표시
                                    $("#per").text((progress.loaded / 1024).toFixed(0).replace(/\B(?=(\d{3})+(?!\d))/g, ",") + 'kb / ' + (progress.total / 1024).toFixed(0).replace(/\B(?=(\d{3})+(?!\d))/g, ",") + 'kb - ' + total + "%");
                                    //진행 막바 표시
                                    $("#progBar").width(total + "%").attr("aria-valuenow", total).text(total + "%");
                                    $(".prog-bar-fill").css("width", total + "%").text(total + "%");
                                }, false);
                                xhr.addEventListener("load", function (evt) {
                                    //alert("loadend");
                                }, false);

                            }

                            return xhr;
                        },
                        success: function (e) {

                            alert("Complete");

                        }
                    }).done(function () {

                        //bfileNum.length = 0;
                        $("#per").text("");
                        clearFileInput();
                        $('#progress-container').empty();
                       

                        $(".prog-bar-fill").css("width", "0%").text("");

                        location.href = "/Movies/Index";

                        
                    });
                }
                
            },
            error: function () {
                alert("데이터를 저장하는 중 오류 발생");

            }
        });



    });


    $(document).on("click", ".divProgress > .abort", function () {

       
        totFiles = 0;
       
        //var idx = Number($(this).parents(0).attr('id'));
        var idx = $(".abort").index(this);
        //alert(fdata.getAll("files").length);
        var afdata = fdata.getAll("files"); 
        afdata.splice(idx, 1);
        //alert(afdata.length);
        fdata.delete("files");

        

        $.each(afdata, function(i, v){
            fdata.append("files", v);
            totFiles = totFiles + v.size;
        });
        //fdata.set("files", afdata);


        $(this).closest(".divProgress").fadeOut(1000, function () {
             $(this).remove();

        });

        $("#per").text((totFiles / 1024).toFixed(0).replace(/\B(?=(\d{3})+(?!\d))/g, ",") + 'kb');

    });


    //파일업로트 테스트
    $('#btnUpload').on('click', function () {

        var frmdata = new FormData();

        
        //폼의 파일 부분
        var files = $('#ajax_file').prop("files");


        for (var i = 0; i < files.length; i++) {
            frmdata.append("files", files[i]);

        }
        if (files.length > 0) {

            $.ajax({
                type: "POST",
                url: "/Movies/Create?handler=Upload&Id=0",
                beforeSend: function (xhr) {
                    xhr.setRequestHeader("XSRF-TOKEN",
                        $('input:hidden[name="__RequestVerificationToken"]').val());
                },
                data: frmdata,
                contentType: false,
                processData: false,
                xhr: function () {

                    var xhr = $.ajaxSettings.xhr();
                    if (xhr.upload) {

                        xhr.upload.addEventListener("progress", function (progress) {
                            var total = Math.round((progress.loaded / progress.total) * 100);
                            //텍스트로 표시
                            $("#per").text((progress.loaded / 1024).toFixed(0).replace(/\B(?=(\d{3})+(?!\d))/g, ",") + 'kb / ' + (progress.total / 1024).toFixed(0).replace(/\B(?=(\d{3})+(?!\d))/g, ",") + 'kb - ' + total + "%");
                            //진행 막바 표시
                            $("#progBar").width(total + "%").attr("aria-valuenow", total).text(total + "%");
                            $(".prog-bar-fill").css("width", total + "%").text(total + "%");
                        }, false);

                        xhr.addEventListener("loadend", function (evt) {

                        }, false);


                    }

                    return xhr;
                },
                success: function (e) {

                    alert("Complete");

                }
            }).done(function () {

                //bfileNum.length = 0;
                $("#per").text("");
                clearFileInput();
                $('#progress-container').empty();


                $(".prog-bar-fill").css("width", "0%").text("");


            });

        }

    });
});
