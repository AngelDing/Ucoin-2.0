<?xml version="1.0" encoding="utf-8"?>
<configurationSectionModel xmlns:dm0="http://schemas.microsoft.com/VisualStudio/2008/DslTools/Core" dslVersion="1.0.0.0" Id="7d354705-7f30-4827-891e-184346ae5ba1" namespace="Ucoin.ServiceModel.Client.Configuration" xmlSchemaNamespace="Ucoin.ServiceModel.Client.Configuration" xmlns="http://schemas.microsoft.com/dsltools/ConfigurationSectionDesigner">
  <typeDefinitions>
    <externalType name="String" namespace="System" />
    <externalType name="Boolean" namespace="System" />
    <externalType name="Int32" namespace="System" />
    <externalType name="Int64" namespace="System" />
    <externalType name="Single" namespace="System" />
    <externalType name="Double" namespace="System" />
    <externalType name="DateTime" namespace="System" />
    <externalType name="TimeSpan" namespace="System" />
  </typeDefinitions>
  <configurationElements>
    <configurationSection name="WcfClientSection" codeGenOptions="Singleton, XmlnsProperty" xmlSectionName="wcf.client">
      <elementProperties>
        <elementProperty name="Services" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="services" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/7d354705-7f30-4827-891e-184346ae5ba1/ServiceCollection" />
          </type>
        </elementProperty>
        <elementProperty name="Clients" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="clients" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/7d354705-7f30-4827-891e-184346ae5ba1/ClientCollection" />
          </type>
        </elementProperty>
      </elementProperties>
    </configurationSection>
    <configurationElementCollection name="ServiceCollection" xmlItemName="service" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods">
      <itemType>
        <configurationElementMoniker name="/7d354705-7f30-4827-891e-184346ae5ba1/ServiceElement" />
      </itemType>
    </configurationElementCollection>
    <configurationElement name="ServiceElement">
      <attributeProperties>
        <attributeProperty name="Assembly" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="assembly" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/7d354705-7f30-4827-891e-184346ae5ba1/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Type" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="type" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/7d354705-7f30-4827-891e-184346ae5ba1/String" />
          </type>
        </attributeProperty>
      </attributeProperties>
    </configurationElement>
    <configurationElementCollection name="ClientCollection" xmlItemName="client" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods">
      <itemType>
        <configurationElementMoniker name="/7d354705-7f30-4827-891e-184346ae5ba1/ClientElement" />
      </itemType>
    </configurationElementCollection>
    <configurationElement name="ClientElement">
      <attributeProperties>
        <attributeProperty name="Address" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="address" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/7d354705-7f30-4827-891e-184346ae5ba1/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Assembly" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="assembly" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/7d354705-7f30-4827-891e-184346ae5ba1/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Type" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="type" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/7d354705-7f30-4827-891e-184346ae5ba1/String" />
          </type>
        </attributeProperty>
      </attributeProperties>
    </configurationElement>
  </configurationElements>
  <propertyValidators>
    <validators />
  </propertyValidators>
</configurationSectionModel>