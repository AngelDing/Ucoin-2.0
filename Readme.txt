VS 2013 GitHub
http://michaelcrump.net/setting-up-github-to-work-with-visual-studio-2013-step-by-step


Ucoin.Framework.ObjectMapper�����_Դ�Ŀ��
https://objectmapper.codeplex.com/


1. �]�Ե�EfRepositoryContext�������L���Ƿ���Ҫ�؄e�O����

2.Code First 
   �����w�ƣ�
   �ք��w�ƣ����� -��NuGet����������� -�����������������̨
   a. Enable-Migrations -ProjectName XXX.YYY.ZZZ -Force
   b. add-migration InitialCreate -ProjectName XXX.YYY.ZZZ
      Add-Migration FirstMigration -ProjectName XXX.YYY.ZZZ
   c. Update-Database -ProjectName XXX.YYY.ZZZ
   �Ԅ��w�ƣ���Migrations.Configuration.cs�У��޸�AutomaticMigrationsEnabled = true;
   �����ڳ��򆢄ӕr�{�� UcoinDBContextInitailizer.Initialize();������app.config�е�<contexts>����헣�
   
   ��Ҫ��ʾ�����õ�朽��ַ����У�name="�����쌍�������Q";��t�����Ԅ�߀���ք��w�ƾ��������ܶ����}��

3. �����D�Q���ܣ� �ք� �� EmitMapper �� ObjectMapper �� AutoMapper

4. Ucoin.Framework.ServiceLocator��ʲ��Ҫ������һ���Ŀ��
   -��֮ǰҲ�����Ucoin.Framework�Ŀ�У�����Ĭ�J���F�ǻ��Unity�ģ��@��Ucoin.Framework��Ҫ���õ�����Unity�M�������˱���Ucoin.Framework
   �Ĺ����ԣ���ò�Ҫ���õ������M�������ServiceLocator���F��ه��������IOC�M�����t��Ҫ��Ӹ���ĵ������M�����ʪ�����һ���Ϊ����Ŀ��

5. MVC��퓣�����_����ӣ� @Html.DoPagingPost(false);���б�ǰ��ӣ�
	<!--���-->
	<div class="toolbar">
		<div class="button">
			<input id="btnSearch" type="submit" value=" Export to Excel "
				   onclick="btnExport('@Url.Action("ExportSettlementApplyList", "Report")')" />
			<input id="btnPrint" type="submit" value=" �� ӡ "
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

6. Log����MongoDb�惦���֞����׷Nlog��
   A. AppLogӛ䛸������\�Еr��һЩ��Ϣ����debug��info��error��trace�ȣ�
   B. ErrorLogӛ�ϵ�yδ���@�Į�����Ϣ����mvc�� api�� wcf�ȣ�
   C. PerfLogӛ��L��mvc�� api�� wcf���P���������ĕr�g�ȣ�
  
7. ServiceStack.Redis��ǰ�汾��4.0.38