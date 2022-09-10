mergeInto(LibraryManager.library, {
    addData:function(jsonData){
        var jsonObj = JSON.parse(Pointer_stringify(jsonData));
        add_data(jsonObj);
    },
    downloadData:function(){
        download_data();
    },
});