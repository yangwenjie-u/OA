// JavaScript Document
function getData(){
	this.menu = "[{oneLevelUrl:'#',aClass:'#',iClass:'fa fa-home',oneSpan:'nav-label',oneSpanText:'主页',twoSpan:'fa arrow',subMenu:[{subUrl:'table_bootstrap.html?v=1',subText:'主页示例一'},{subUrl:'table_bootstrap.html?v=2',subText:'主页示例二'},{subUrl:'table_bootstrap.html?v=3',subText:'主页示例三'}]},{oneLevelUrl:'table_bootstrap.html?v=4',aClass:'J_menuItem',iClass:'fa fa-columns',oneSpan:'nav-label',oneSpanText:'布局',twoSpan:'#',subMenu:[]},{oneLevelUrl:'#',aClass:'#',iClass:'fa fa-desktop',oneSpan:'nav-label',oneSpanText:'页面',twoSpan:'fa arrow',subMenu:[{subUrl:'table_bootstrap.html',subText:'客户管理'}]}]";
	this.listdata = [
    {
        "id": "1",
        "name": "张三",
        "age": "25",
        "height": "65",
        "weight": "180",
        "idCard": "330602199212129876",
        "address": "浙江绍兴",
        "phone": "15088551234"
    },
    {
        "id": "2",
        "name": "李四",
        "age": "21",
        "height": "70",
        "weight": "170",
        "idCard": "330602199011121234",
        "address": "浙江绍兴",
        "phone": "12482351134"
    },
    {
        "id": "3",
        "name": "王五",
        "age": "29",
        "height": "66",
        "weight": "176",
        "idCard": "330602198602182671",
        "address": "浙江金华",
        "phone": "15487199820"
    },
    {
        "id": "4",
        "name": "赵六",
        "age": "20",
        "height": "67",
        "weight": "177",
        "idCard": "3306021995109128719",
        "address": "浙江杭州",
        "phone": "1678911120"
    },
    {
        "id": "5",
        "name": "钱七",
        "age": "25",
        "height": "62",
        "weight": "180",
        "idCard": "330602199206129810",
        "address": "浙江绍兴",
        "phone": "12787811235"
    },
    {
        "id": "6",
        "name": "孙八",
        "age": "27",
        "height": "164",
        "weight": "55",
        "idCard": "330602199004191926",
        "address": "浙江义乌",
        "phone": "15712896102"
    },
    {
        "id": "7",
        "name": "杨九",
        "age": "54",
        "height": "65",
        "weight": "170",
        "idCard": "330602196511223498",
        "address": "浙江绍兴",
        "phone": "15988172011"
    },
    {
        "id": "8",
        "name": "吴十",
        "age": "26",
        "height": "80",
        "weight": "185",
        "idCard": "330602199108091122",
        "address": "浙江绍兴",
        "phone": "15116751124"
    },
    {
        "id": "9",
        "name": "陈一",
        "age": "25",
        "height": "65",
        "weight": "180",
        "idCard": "330602199212129876",
        "address": "浙江绍兴",
        "phone": "15088551234"
    },
    {
        "id": "10",
        "name": "黄二",
        "age": "25",
        "height": "65",
        "weight": "180",
        "idCard": "330602199212129876",
        "address": "浙江绍兴",
        "phone": "15088551234"
    },{
        "id": "11",
        "name": "张三1",
        "age": "25",
        "height": "65",
        "weight": "180",
        "idCard": "330602199212129876",
        "address": "浙江绍兴",
        "phone": "15088551234"
    },
    {
        "id": "12",
        "name": "李四1",
        "age": "21",
        "height": "70",
        "weight": "170",
        "idCard": "330602199011121234",
        "address": "浙江绍兴",
        "phone": "12482351134"
    },
    {
        "id": "13",
        "name": "王五1",
        "age": "29",
        "height": "66",
        "weight": "176",
        "idCard": "330602198602182671",
        "address": "浙江金华",
        "phone": "15487199820"
    },
    {
        "id": "14",
        "name": "赵六1",
        "age": "20",
        "height": "67",
        "weight": "177",
        "idCard": "3306021995109128719",
        "address": "浙江杭州",
        "phone": "1678911120"
    },
    {
        "id": "15",
        "name": "钱七1",
        "age": "25",
        "height": "62",
        "weight": "180",
        "idCard": "330602199206129810",
        "address": "浙江绍兴",
        "phone": "12787811235"
    },
    {
        "id": "16",
        "name": "孙八1",
        "age": "27",
        "height": "164",
        "weight": "55",
        "idCard": "330602199004191926",
        "address": "浙江义乌",
        "phone": "15712896102"
    },
    {
        "id": "17",
        "name": "杨九1",
        "age": "54",
        "height": "65",
        "weight": "170",
        "idCard": "330602196511223498",
        "address": "浙江绍兴",
        "phone": "15988172011"
    },
    {
        "id": "18",
        "name": "吴十1",
        "age": "26",
        "height": "80",
        "weight": "185",
        "idCard": "330602199108091122",
        "address": "浙江绍兴",
        "phone": "15116751124"
    },
    {
        "id": "19",
        "name": "陈一1",
        "age": "25",
        "height": "65",
        "weight": "180",
        "idCard": "330602199212129876",
        "address": "浙江绍兴",
        "phone": "15088551234"
    },
    {
        "id": "20",
        "name": "黄二1",
        "age": "25",
        "height": "65",
        "weight": "180",
        "idCard": "330602199212129876",
        "address": "浙江绍兴",
        "phone": "15088551234"
    },{
        "id": "21",
        "name": "张三2",
        "age": "25",
        "height": "65",
        "weight": "180",
        "idCard": "330602199212129876",
        "address": "浙江绍兴",
        "phone": "15088551234"
    },
    {
        "id": "22",
        "name": "李四2",
        "age": "21",
        "height": "70",
        "weight": "170",
        "idCard": "330602199011121234",
        "address": "浙江绍兴",
        "phone": "12482351134"
    },
    {
        "id": "23",
        "name": "王五2",
        "age": "29",
        "height": "66",
        "weight": "176",
        "idCard": "330602198602182671",
        "address": "浙江金华",
        "phone": "15487199820"
    },
    {
        "id": "24",
        "name": "赵六2",
        "age": "20",
        "height": "67",
        "weight": "177",
        "idCard": "3306021995109128719",
        "address": "浙江杭州",
        "phone": "1678911120"
    },
    {
        "id": "25",
        "name": "钱七2",
        "age": "25",
        "height": "62",
        "weight": "180",
        "idCard": "330602199206129810",
        "address": "浙江绍兴",
        "phone": "12787811235"
    },
    {
        "id": "26",
        "name": "孙八2",
        "age": "27",
        "height": "164",
        "weight": "55",
        "idCard": "330602199004191926",
        "address": "浙江义乌",
        "phone": "15712896102"
    },
    {
        "id": "27",
        "name": "杨九2",
        "age": "54",
        "height": "65",
        "weight": "170",
        "idCard": "330602196511223498",
        "address": "浙江绍兴",
        "phone": "15988172011"
    },
    {
        "id": "28",
        "name": "吴十2",
        "age": "26",
        "height": "80",
        "weight": "185",
        "idCard": "330602199108091122",
        "address": "浙江绍兴",
        "phone": "15116751124"
    },
    {
        "id": "29",
        "name": "陈一2",
        "age": "25",
        "height": "65",
        "weight": "180",
        "idCard": "330602199212129876",
        "address": "浙江绍兴",
        "phone": "15088551234"
    },
    {
        "id": "30",
        "name": "黄二2",
        "age": "25",
        "height": "65",
        "weight": "180",
        "idCard": "330602199212129876",
        "address": "浙江绍兴",
        "phone": "15088551234"
    },{
        "id": "31",
        "name": "张三3",
        "age": "25",
        "height": "65",
        "weight": "180",
        "idCard": "330602199212129876",
        "address": "浙江绍兴",
        "phone": "15088551234"
    },
    {
        "id": "32",
        "name": "李四3",
        "age": "21",
        "height": "70",
        "weight": "170",
        "idCard": "330602199011121234",
        "address": "浙江绍兴",
        "phone": "12482351134"
    },
    {
        "id": "33",
        "name": "王五3",
        "age": "29",
        "height": "66",
        "weight": "176",
        "idCard": "330602198602182671",
        "address": "浙江金华",
        "phone": "15487199820"
    },
    {
        "id": "34",
        "name": "赵六3",
        "age": "20",
        "height": "67",
        "weight": "177",
        "idCard": "3306021995109128719",
        "address": "浙江杭州",
        "phone": "1678911120"
    },
    {
        "id": "35",
        "name": "钱七3",
        "age": "25",
        "height": "62",
        "weight": "180",
        "idCard": "330602199206129810",
        "address": "浙江绍兴",
        "phone": "12787811235"
    },
    {
        "id": "36",
        "name": "孙八3",
        "age": "27",
        "height": "164",
        "weight": "55",
        "idCard": "330602199004191926",
        "address": "浙江义乌",
        "phone": "15712896102"
    },
    {
        "id": "37",
        "name": "杨九3",
        "age": "54",
        "height": "65",
        "weight": "170",
        "idCard": "330602196511223498",
        "address": "浙江绍兴",
        "phone": "15988172011"
    },
    {
        "id": "38",
        "name": "吴十3",
        "age": "26",
        "height": "80",
        "weight": "185",
        "idCard": "330602199108091122",
        "address": "浙江绍兴",
        "phone": "15116751124"
    },
    {
        "id": "39",
        "name": "陈一3",
        "age": "25",
        "height": "65",
        "weight": "180",
        "idCard": "330602199212129876",
        "address": "浙江绍兴",
        "phone": "15088551234"
    },
    {
        "id": "40",
        "name": "黄二3",
        "age": "25",
        "height": "65",
        "weight": "180",
        "idCard": "330602199212129876",
        "address": "浙江绍兴",
        "phone": "15088551234"
    },{
        "id": "41",
        "name": "张三4",
        "age": "25",
        "height": "65",
        "weight": "180",
        "idCard": "330602199212129876",
        "address": "浙江绍兴",
        "phone": "15088551234"
    },
    {
        "id": "42",
        "name": "李四4",
        "age": "21",
        "height": "70",
        "weight": "170",
        "idCard": "330602199011121234",
        "address": "浙江绍兴",
        "phone": "12482351134"
    },
    {
        "id": "43",
        "name": "王五4",
        "age": "29",
        "height": "66",
        "weight": "176",
        "idCard": "330602198602182671",
        "address": "浙江金华",
        "phone": "15487199820"
    },
    {
        "id": "44",
        "name": "赵六4",
        "age": "20",
        "height": "67",
        "weight": "177",
        "idCard": "3306021995109128719",
        "address": "浙江杭州",
        "phone": "1678911120"
    },
    {
        "id": "45",
        "name": "钱七4",
        "age": "25",
        "height": "62",
        "weight": "180",
        "idCard": "330602199206129810",
        "address": "浙江绍兴",
        "phone": "12787811235"
    },
    {
        "id": "46",
        "name": "孙八4",
        "age": "27",
        "height": "164",
        "weight": "55",
        "idCard": "330602199004191926",
        "address": "浙江义乌",
        "phone": "15712896102"
    },
    {
        "id": "47",
        "name": "杨九4",
        "age": "54",
        "height": "65",
        "weight": "170",
        "idCard": "330602196511223498",
        "address": "浙江绍兴",
        "phone": "15988172011"
    },
    {
        "id": "48",
        "name": "吴十4",
        "age": "26",
        "height": "80",
        "weight": "185",
        "idCard": "330602199108091122",
        "address": "浙江绍兴",
        "phone": "15116751124"
    },
    {
        "id": "49",
        "name": "陈一4",
        "age": "25",
        "height": "65",
        "weight": "180",
        "idCard": "330602199212129876",
        "address": "浙江绍兴",
        "phone": "15088551234"
    },
    {
        "id": "50",
        "name": "黄二4",
        "age": "25",
        "height": "65",
        "weight": "180",
        "idCard": "330602199212129876",
        "address": "浙江绍兴",
        "phone": "15088551234"
    }
	];
	this.data3 = "c";
	this.data4 = "d";
	this.data5 = "e";
	this.data6 = "f";
	}
	
	function b(){
		alert("b");
		}