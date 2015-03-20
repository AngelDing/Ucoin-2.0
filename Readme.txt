VS 2013 GitHub
http://michaelcrump.net/setting-up-github-to-work-with-visual-studio-2013-step-by-step


Ucoin.Framework.ObjectMapper引用開源項目：
https://objectmapper.codeplex.com/


1. 註冊的EfRepositoryContext，生命週期是否需要特別設定？

2.Code First 
   數據遷移：
   手動遷移：工具 -》NuGet程序包管理其 -》程序包管理器控制台
   a. Enable-Migrations -ProjectName XXX.YYY.ZZZ -Force
   b. add-migration InitialCreate -ProjectName XXX.YYY.ZZZ
      Add-Migration FirstMigration -ProjectName XXX.YYY.ZZZ
   c. Update-Database -ProjectName XXX.YYY.ZZZ
   自動遷移：在Migrations.Configuration.cs中，修改AutomaticMigrationsEnabled = true;
   另外在程序啟動時調用 UcoinDBContextInitailizer.Initialize();并禁用app.config中的<contexts>配置項；
   
   重要提示：配置的鏈接字符串中，name="數據庫實例的名稱";否則不管自動還是手動遷移均會碰到很多問題；

3. 對象轉換性能： 手動 》 EmitMapper 》 ObjectMapper 》 AutoMapper

4. Ucoin.Framework.ServiceLocator爲什麽要獨立成一個項目？
   -》之前也想放在Ucoin.Framework項目中，但是默認實現是基於Unity的，這樣Ucoin.Framework需要引用第三的Unity組件，爲了保持Ucoin.Framework
   的公用性，最好不要引用第三方組件，如果ServiceLocator實現依賴于其他的IOC組件，則需要添加更多的第三方組件，故獨立出一個單獨的項目。

5. MVC分頁：頁面腳本添加： @Html.DoPagingPost(false);，列表前添加：
	<!--分頁-->
	<div class="toolbar">
		<div class="button">
			<input id="btnSearch" type="submit" value=" Export to Excel "
				   onclick="btnExport('@Url.Action("ExportSettlementApplyList", "Report")')" />
			<input id="btnPrint" type="submit" value=" 列 印 "
				   onclick="btnPrint('@Url.Action("SettlementApplyListPrintView", "Report")')" />
		</div>
		<div class="paging">
			@Html.PageLinks(
				new PagingInfo
				{
					CurrentPage = Model.SettlementApplyRQ.PageIndex,
					ItemsPerPage = Model.SettlementApplyRQ.PageSize,
					TotalItems = Model.SettlementApplyRS.TotalCount,
					PageSizeName = "SettlementApplyRQ_PageSize",
					IsAjaxPost = true
				},
				null,
				(x) => x.ToString(),
				null,
				true,
				"SettlementApplyRQ_PageIndex",
				PagingMode.Hybird)
		</div>
	</div>

6. Log採用MongoDb存儲，分為以下幾種log：
   A. AppLog記錄各程序運行時的一些信息，分debug，info，error，trace等；
   B. ErrorLog記錄系統未捕獲的異常信息，為mvc， api， wcf等；
   C. PerfLog記錄訪問mvc， api， wcf相關方法所化的時間等；
  
7. ServiceStack.Redis當前版本：4.0.38