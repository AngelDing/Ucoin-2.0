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