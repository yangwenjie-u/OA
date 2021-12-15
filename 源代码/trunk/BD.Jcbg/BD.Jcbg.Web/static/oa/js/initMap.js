var map; //= new BMap.Map("map");
function initMap() {
    // 百度地图API功能
    map = new BMap.Map("map"); // 创建Map实例
    // 初始化地图,设置中心点坐标和地图级别
    map.centerAndZoom(new BMap.Point(116.404, 39.915), 11);
    //添加地图类型控件
    map.addControl(new BMap.MapTypeControl({
        // mapTypes: [
        //     BMAP_NORMAL_MAP,
        // ],
        anchor: BMAP_ANCHOR_TOP_RIGHT,
        offset: new BMap.Size(10, 50)
    }));
    var top_right_navigation = new BMap.NavigationControl({
        anchor: BMAP_ANCHOR_TOP_RIGHT,
        type: BMAP_NAVIGATION_CONTROL_SMALL,
        offset: new BMap.Size(50, 80)
    }); //右上角，仅包含平移和缩放按钮
    map.addControl(top_right_navigation);
    map.centerAndZoom("台州市", 9); // 设置地图显示的城市 此项是必须设置的
}


function getBoundary() {
    var bdary = new BMap.Boundary();
    bdary.get("台州市", function(rs) { //获取行政区域
        var count = rs.boundaries.length; //行政区域的点有多少个
        if (count === 0) {
            alert('未能获取当前输入行政区域');
            return;
        }
        var pointArray = [];
        for (var i = 0; i < count; i++) {
            var ply = new BMap.Polygon(rs.boundaries[i], { strokeWeight: 2, strokeColor: "#EF7F44", fillColor: "#EF7F44", fillOpacity: 0.1 }); //建立多边形覆盖物
            map.addOverlay(ply); //添加覆盖物
            ply.addEventListener("click",function(){
                map.closeInfoWindow();
            });
            ply.disableMassClear();//禁止清除覆盖物

            pointArray = pointArray.concat(ply.getPath());
        }
        // 
        //map.setViewport(pointArray);    //调整视野      
    });
}

