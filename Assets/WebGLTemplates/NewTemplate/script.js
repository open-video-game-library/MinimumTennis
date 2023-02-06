// データを配列で管理
var datas = [];

// データを追加
function add_data(jsonObj) {
    jsonObj.id = datas.length + 1;
    datas.push(jsonObj);
}

// データをcsv形式にし、ダウンロード
function download_data() {
    let loghead = "id,winner,character1_game_count,character2_game_count,character1_net_count,character2_net_count,character1_out_count,character2_out_count,character1_twobound_count,character2_twobound_count,character1_doublefault_count,character2_doublefault_count,max_rally_count\n";
    let logdata = "";
    datas.map(function (d) {
        logdata += d.id + "," + d.winner + "," + d.character1GameCount + "," + d.character2GameCount
         + "," + d.character1NetCount + "," + d.character2NetCount + "," + d.character1OutCount + "," + d.character2OutCount
         + "," + d.character1TwoBoundCount + "," + d.character2TwoBoundCount + "," + d.character1DoubleFaultCount + "," + d.character2DoubleFaultCount
         + "," + d.maxRallyCount
         + "\n";
    });

    const filename = getNow() + ".csv";
    const bom = new Uint8Array([0xef, 0xbb, 0xbf]);
    const blob = new Blob([bom, loghead + logdata], { type: "text/csv" });

    if (window.navigator.msSaveBlob) {
        window.navigator.msSaveBlob(blob, filename);
    } else {
        const url = (window.URL || window.webkitURL).createObjectURL(blob);
        const download = document.createElement("a");
        download.href = url;
        download.download = filename;
        download.click();
        (window.URL || window.webkitURL).revokeObjectURL(url);
    }
}

// 時刻を取得
function getNow() {
    var date = new Date();
    var yyyy = date.getFullYear();
    var mm = toDoubleDigits(date.getMonth() + 1);
    var dd = toDoubleDigits(date.getDate());
    var hh = toDoubleDigits(date.getHours());
    var mi = toDoubleDigits(date.getMinutes());
    return yyyy + mm + dd + '_' + hh + mi;
}
function toDoubleDigits(num) {
    num += "";
    if (num.length === 1) {
        num = "0" + num;
    }
    return num;   
}