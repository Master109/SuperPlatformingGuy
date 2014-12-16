
var Module;
if (typeof Module === 'undefined') Module = eval('(function() { try { return Module || {} } catch(e) { return {} } })()');
if (!Module.expectedDataFileDownloads) {
  Module.expectedDataFileDownloads = 0;
  Module.finishedDataFileDownloads = 0;
}
Module.expectedDataFileDownloads++;
(function() {

    var PACKAGE_PATH;
    if (typeof window === 'object') {
      PACKAGE_PATH = window['encodeURIComponent'](window.location.pathname.toString().substring(0, window.location.pathname.toString().lastIndexOf('/')) + '/');
    } else {
      // worker
      PACKAGE_PATH = encodeURIComponent(location.pathname.toString().substring(0, location.pathname.toString().lastIndexOf('/')) + '/');
    }
    var PACKAGE_NAME = 'WebGL.data';
    var REMOTE_PACKAGE_BASE = 'WebGL.data';
    if (typeof Module['locateFilePackage'] === 'function' && !Module['locateFile']) {
      Module['locateFile'] = Module['locateFilePackage'];
      Module.printErr('warning: you defined Module.locateFilePackage, that has been renamed to Module.locateFile (using your locateFilePackage for now)');
    }
    var REMOTE_PACKAGE_NAME = typeof Module['locateFile'] === 'function' ?
                              Module['locateFile'](REMOTE_PACKAGE_BASE) :
                              ((Module['filePackagePrefixURL'] || '') + REMOTE_PACKAGE_BASE);
    var REMOTE_PACKAGE_SIZE = 42828933;
    var PACKAGE_UUID = '46b6f4fd-53a0-4b94-80f6-bfea4913c1dd';
  
    function fetchRemotePackage(packageName, packageSize, callback, errback) {
      var xhr = new XMLHttpRequest();
      xhr.open('GET', packageName, true);
      xhr.responseType = 'arraybuffer';
      xhr.onprogress = function(event) {
        var url = packageName;
        var size = packageSize;
        if (event.total) size = event.total;
        if (event.loaded) {
          if (!xhr.addedTotal) {
            xhr.addedTotal = true;
            if (!Module.dataFileDownloads) Module.dataFileDownloads = {};
            Module.dataFileDownloads[url] = {
              loaded: event.loaded,
              total: size
            };
          } else {
            Module.dataFileDownloads[url].loaded = event.loaded;
          }
          var total = 0;
          var loaded = 0;
          var num = 0;
          for (var download in Module.dataFileDownloads) {
          var data = Module.dataFileDownloads[download];
            total += data.total;
            loaded += data.loaded;
            num++;
          }
          total = Math.ceil(total * Module.expectedDataFileDownloads/num);
          if (Module['setStatus']) Module['setStatus']('Downloading data... (' + loaded + '/' + total + ')');
        } else if (!Module.dataFileDownloads) {
          if (Module['setStatus']) Module['setStatus']('Downloading data...');
        }
      };
      xhr.onload = function(event) {
        var packageData = xhr.response;
        callback(packageData);
      };
      xhr.send(null);
    };

    function handleError(error) {
      console.error('package error:', error);
    };
  
      var fetched = null, fetchedCallback = null;
      fetchRemotePackage(REMOTE_PACKAGE_NAME, REMOTE_PACKAGE_SIZE, function(data) {
        if (fetchedCallback) {
          fetchedCallback(data);
          fetchedCallback = null;
        } else {
          fetched = data;
        }
      }, handleError);
    
  function runWithFS() {

function assert(check, msg) {
  if (!check) throw msg + new Error().stack;
}
Module['FS_createPath']('/', 'Resources', true, true);

    function DataRequest(start, end, crunched, audio) {
      this.start = start;
      this.end = end;
      this.crunched = crunched;
      this.audio = audio;
    }
    DataRequest.prototype = {
      requests: {},
      open: function(mode, name) {
        this.name = name;
        this.requests[name] = this;
        Module['addRunDependency']('fp ' + this.name);
      },
      send: function() {},
      onload: function() {
        var byteArray = this.byteArray.subarray(this.start, this.end);

          this.finish(byteArray);

      },
      finish: function(byteArray) {
        var that = this;
        Module['FS_createPreloadedFile'](this.name, null, byteArray, true, true, function() {
          Module['removeRunDependency']('fp ' + that.name);
        }, function() {
          if (that.audio) {
            Module['removeRunDependency']('fp ' + that.name); // workaround for chromium bug 124926 (still no audio with this, but at least we don't hang)
          } else {
            Module.printErr('Preloading file ' + that.name + ' failed');
          }
        }, false, true); // canOwn this data in the filesystem, it is a slide into the heap that will never change
        this.requests[this.name] = null;
      },
    };
      new DataRequest(0, 8358, 0, 0).open('GET', '/1a4e66c843f944f168a72c72863d1721.resource');
    new DataRequest(8358, 14626, 0, 0).open('GET', '/3275178839e1c42228bc94d29c440da3.resource');
    new DataRequest(14626, 1040296, 0, 0).open('GET', '/5c5fb4b0dcf5a4c59929598cd76867ea.resource');
    new DataRequest(1040296, 1047818, 0, 0).open('GET', '/64c6b9e5ae64144bea9c664867caf9b4.resource');
    new DataRequest(1047818, 1061191, 0, 0).open('GET', '/8838d2ec430214bc39b1d7b317a853b0.resource');
    new DataRequest(1061191, 1068295, 0, 0).open('GET', '/89f11e75520b94be1bf2b0438198c870.resource');
    new DataRequest(1068295, 1079996, 0, 0).open('GET', '/904640fde1d9d404d8f2602fb4c06d2e.resource');
    new DataRequest(1079996, 1087100, 0, 0).open('GET', '/9a77d3fd48759404d959818a09434347.resource');
    new DataRequest(1087100, 1095458, 0, 0).open('GET', '/9f3224d3ee2024d249c028feacc347b5.resource');
    new DataRequest(1095458, 1105070, 0, 0).open('GET', '/b747918a0256e4f8988110e75bbea165.resource');
    new DataRequest(1105070, 1112592, 0, 0).open('GET', '/cb56055dcf8564c558277cc5072c640b.resource');
    new DataRequest(1112592, 1121368, 0, 0).open('GET', '/d1dc3f4ef9f704eb38fba8bcb3c9ab83.resource');
    new DataRequest(1121368, 1129726, 0, 0).open('GET', '/d9adf7ddb8fff4b229e344f501e38c91.resource');
    new DataRequest(1129726, 1141009, 0, 0).open('GET', '/dac7d22fff8d24dfbba3b8f3aeb5aefb.resource');
    new DataRequest(1141009, 1149785, 0, 0).open('GET', '/dcdff6e2a692144ff94042cc125e1129.resource');
    new DataRequest(1149785, 1159397, 0, 0).open('GET', '/f11d6300c2b3e468c89fed7963e36404.resource');
    new DataRequest(1159397, 2356013, 0, 0).open('GET', '/f1c2602b4fad2489cb4d55e486b8095a.resource');
    new DataRequest(2356013, 2540665, 0, 0).open('GET', '/level0');
    new DataRequest(2540665, 3272101, 0, 0).open('GET', '/level1');
    new DataRequest(3272101, 3475385, 0, 0).open('GET', '/level10');
    new DataRequest(3475385, 3713781, 0, 0).open('GET', '/level11');
    new DataRequest(3713781, 4098305, 0, 0).open('GET', '/level12');
    new DataRequest(4098305, 4313109, 0, 0).open('GET', '/level13');
    new DataRequest(4313109, 4611145, 0, 0).open('GET', '/level14');
    new DataRequest(4611145, 4971749, 0, 0).open('GET', '/level15');
    new DataRequest(4971749, 5619089, 0, 0).open('GET', '/level16');
    new DataRequest(5619089, 6361293, 0, 0).open('GET', '/level17');
    new DataRequest(6361293, 7431881, 0, 0).open('GET', '/level18');
    new DataRequest(7431881, 7715181, 0, 0).open('GET', '/level2');
    new DataRequest(7715181, 7907681, 0, 0).open('GET', '/level3');
    new DataRequest(7907681, 8140837, 0, 0).open('GET', '/level4');
    new DataRequest(8140837, 8382461, 0, 0).open('GET', '/level5');
    new DataRequest(8382461, 8630529, 0, 0).open('GET', '/level6');
    new DataRequest(8630529, 8938981, 0, 0).open('GET', '/level7');
    new DataRequest(8938981, 9405945, 0, 0).open('GET', '/level8');
    new DataRequest(9405945, 9922917, 0, 0).open('GET', '/level9');
    new DataRequest(9922917, 10567049, 0, 0).open('GET', '/mainData');
    new DataRequest(10567049, 33121577, 0, 0).open('GET', '/sharedassets0.assets');
    new DataRequest(33121577, 33126609, 0, 0).open('GET', '/sharedassets1.assets');
    new DataRequest(33126609, 33131701, 0, 0).open('GET', '/sharedassets10.assets');
    new DataRequest(33131701, 33297233, 0, 0).open('GET', '/sharedassets11.assets');
    new DataRequest(33297233, 33302181, 0, 0).open('GET', '/sharedassets12.assets');
    new DataRequest(33302181, 36305525, 0, 0).open('GET', '/sharedassets13.assets');
    new DataRequest(36305525, 36310653, 0, 0).open('GET', '/sharedassets14.assets');
    new DataRequest(36310653, 36315661, 0, 0).open('GET', '/sharedassets15.assets');
    new DataRequest(36315661, 36320753, 0, 0).open('GET', '/sharedassets16.assets');
    new DataRequest(36320753, 36325833, 0, 0).open('GET', '/sharedassets17.assets');
    new DataRequest(36325833, 36330913, 0, 0).open('GET', '/sharedassets18.assets');
    new DataRequest(36330913, 36335969, 0, 0).open('GET', '/sharedassets19.assets');
    new DataRequest(36335969, 38658649, 0, 0).open('GET', '/sharedassets2.assets');
    new DataRequest(38658649, 39193229, 0, 0).open('GET', '/sharedassets3.assets');
    new DataRequest(39193229, 41837141, 0, 0).open('GET', '/sharedassets4.assets');
    new DataRequest(41837141, 41842197, 0, 0).open('GET', '/sharedassets5.assets');
    new DataRequest(41842197, 41847205, 0, 0).open('GET', '/sharedassets6.assets');
    new DataRequest(41847205, 41852321, 0, 0).open('GET', '/sharedassets7.assets');
    new DataRequest(41852321, 41857365, 0, 0).open('GET', '/sharedassets8.assets');
    new DataRequest(41857365, 41862373, 0, 0).open('GET', '/sharedassets9.assets');
    new DataRequest(41862373, 42525173, 0, 0).open('GET', '/Resources/unity_default_resources');
    new DataRequest(42525173, 42828933, 0, 0).open('GET', '/Resources/unity_builtin_extra');

    function processPackageData(arrayBuffer) {
      Module.finishedDataFileDownloads++;
      assert(arrayBuffer, 'Loading data file failed.');
      var byteArray = new Uint8Array(arrayBuffer);
      var curr;
      
      // Reuse the bytearray from the XHR as the source for file reads.
      DataRequest.prototype.byteArray = byteArray;
          DataRequest.prototype.requests["/1a4e66c843f944f168a72c72863d1721.resource"].onload();
          DataRequest.prototype.requests["/3275178839e1c42228bc94d29c440da3.resource"].onload();
          DataRequest.prototype.requests["/5c5fb4b0dcf5a4c59929598cd76867ea.resource"].onload();
          DataRequest.prototype.requests["/64c6b9e5ae64144bea9c664867caf9b4.resource"].onload();
          DataRequest.prototype.requests["/8838d2ec430214bc39b1d7b317a853b0.resource"].onload();
          DataRequest.prototype.requests["/89f11e75520b94be1bf2b0438198c870.resource"].onload();
          DataRequest.prototype.requests["/904640fde1d9d404d8f2602fb4c06d2e.resource"].onload();
          DataRequest.prototype.requests["/9a77d3fd48759404d959818a09434347.resource"].onload();
          DataRequest.prototype.requests["/9f3224d3ee2024d249c028feacc347b5.resource"].onload();
          DataRequest.prototype.requests["/b747918a0256e4f8988110e75bbea165.resource"].onload();
          DataRequest.prototype.requests["/cb56055dcf8564c558277cc5072c640b.resource"].onload();
          DataRequest.prototype.requests["/d1dc3f4ef9f704eb38fba8bcb3c9ab83.resource"].onload();
          DataRequest.prototype.requests["/d9adf7ddb8fff4b229e344f501e38c91.resource"].onload();
          DataRequest.prototype.requests["/dac7d22fff8d24dfbba3b8f3aeb5aefb.resource"].onload();
          DataRequest.prototype.requests["/dcdff6e2a692144ff94042cc125e1129.resource"].onload();
          DataRequest.prototype.requests["/f11d6300c2b3e468c89fed7963e36404.resource"].onload();
          DataRequest.prototype.requests["/f1c2602b4fad2489cb4d55e486b8095a.resource"].onload();
          DataRequest.prototype.requests["/level0"].onload();
          DataRequest.prototype.requests["/level1"].onload();
          DataRequest.prototype.requests["/level10"].onload();
          DataRequest.prototype.requests["/level11"].onload();
          DataRequest.prototype.requests["/level12"].onload();
          DataRequest.prototype.requests["/level13"].onload();
          DataRequest.prototype.requests["/level14"].onload();
          DataRequest.prototype.requests["/level15"].onload();
          DataRequest.prototype.requests["/level16"].onload();
          DataRequest.prototype.requests["/level17"].onload();
          DataRequest.prototype.requests["/level18"].onload();
          DataRequest.prototype.requests["/level2"].onload();
          DataRequest.prototype.requests["/level3"].onload();
          DataRequest.prototype.requests["/level4"].onload();
          DataRequest.prototype.requests["/level5"].onload();
          DataRequest.prototype.requests["/level6"].onload();
          DataRequest.prototype.requests["/level7"].onload();
          DataRequest.prototype.requests["/level8"].onload();
          DataRequest.prototype.requests["/level9"].onload();
          DataRequest.prototype.requests["/mainData"].onload();
          DataRequest.prototype.requests["/sharedassets0.assets"].onload();
          DataRequest.prototype.requests["/sharedassets1.assets"].onload();
          DataRequest.prototype.requests["/sharedassets10.assets"].onload();
          DataRequest.prototype.requests["/sharedassets11.assets"].onload();
          DataRequest.prototype.requests["/sharedassets12.assets"].onload();
          DataRequest.prototype.requests["/sharedassets13.assets"].onload();
          DataRequest.prototype.requests["/sharedassets14.assets"].onload();
          DataRequest.prototype.requests["/sharedassets15.assets"].onload();
          DataRequest.prototype.requests["/sharedassets16.assets"].onload();
          DataRequest.prototype.requests["/sharedassets17.assets"].onload();
          DataRequest.prototype.requests["/sharedassets18.assets"].onload();
          DataRequest.prototype.requests["/sharedassets19.assets"].onload();
          DataRequest.prototype.requests["/sharedassets2.assets"].onload();
          DataRequest.prototype.requests["/sharedassets3.assets"].onload();
          DataRequest.prototype.requests["/sharedassets4.assets"].onload();
          DataRequest.prototype.requests["/sharedassets5.assets"].onload();
          DataRequest.prototype.requests["/sharedassets6.assets"].onload();
          DataRequest.prototype.requests["/sharedassets7.assets"].onload();
          DataRequest.prototype.requests["/sharedassets8.assets"].onload();
          DataRequest.prototype.requests["/sharedassets9.assets"].onload();
          DataRequest.prototype.requests["/Resources/unity_default_resources"].onload();
          DataRequest.prototype.requests["/Resources/unity_builtin_extra"].onload();
          Module['removeRunDependency']('datafile_WebGL.data');

    };
    Module['addRunDependency']('datafile_WebGL.data');
  
    if (!Module.preloadResults) Module.preloadResults = {};
  
      Module.preloadResults[PACKAGE_NAME] = {fromCache: false};
      if (fetched) {
        processPackageData(fetched);
        fetched = null;
      } else {
        fetchedCallback = processPackageData;
      }
    
  }
  if (Module['calledRun']) {
    runWithFS();
  } else {
    if (!Module['preRun']) Module['preRun'] = [];
    Module["preRun"].push(runWithFS); // FS is not initialized yet, wait for it
  }

})();
