private void btnFedexRate_Click(object sender, EventArgs e)
{
    MessageBox.Show("1@" + TLS_waybill.paphone + "2@" + TLS_waybill.baphone + "3@" + TLS_waybill.bphone + "4@" + TLS_waybill.haphone + "5@" + TLS_waybill.hphone + "6@" + TLS_waybill.pphone + "7@" + TLS_waybill.saphone + "8@" + TLS_waybill.sphone);

    int num1 = 1;
    Convert.ToInt32(TLS_waybill.c_pieces);
    string url = "https://ws.fedex.com:443/web-services";
    DataTable dataTable = TLS.Data_Waybill_Cube(TLS.g_waybill, "SC");
    XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
    using (XmlWriter xmlWriter = XmlWriter.Create(AppDomain.CurrentDomain.BaseDirectory + "\\logs\\request_" + (object)num1 + ".xml", new XmlWriterSettings()
    {
        ConformanceLevel = ConformanceLevel.Fragment,
        OmitXmlDeclaration = true
    }))
    {
        xmlWriter.WriteStartElement("RateRequest", "http://fedex.com/ws/rate/v24");
        xmlWriter.WriteAttributeString("xmlns", "http://fedex.com/ws/rate/v24");
        xmlWriter.WriteStartElement("WebAuthenticationDetail");
        xmlWriter.WriteStartElement("UserCredential");
        xmlWriter.WriteElementString("Key", "");
        xmlWriter.WriteElementString("Password", "");
        xmlWriter.WriteEndElement();
        xmlWriter.WriteEndElement();
        xmlWriter.WriteStartElement("ClientDetail");
        xmlWriter.WriteElementString("AccountNumber", "");
        xmlWriter.WriteElementString("MeterNumber", "");
        xmlWriter.WriteEndElement();
        xmlWriter.WriteStartElement("Version");
        xmlWriter.WriteElementString("ServiceId", "crs");
        xmlWriter.WriteElementString("Major", "24");
        xmlWriter.WriteElementString("Intermediate", "0");
        xmlWriter.WriteElementString("Minor", "0");
        xmlWriter.WriteEndElement();
        xmlWriter.WriteStartElement("RequestedShipment");
        xmlWriter.WriteStartElement("Recipient");
        xmlWriter.WriteStartElement("Address");
        xmlWriter.WriteElementString("StreetLines", TLS_waybill.saddress1);
        xmlWriter.WriteElementString("City", TLS_waybill.scity);
        xmlWriter.WriteElementString("StateOrProvinceCode", TLS_waybill.sstate);
        xmlWriter.WriteElementString("PostalCode", TLS_waybill.szip);
        xmlWriter.WriteElementString("CountryCode", TLS_waybill.scountry);
        if (this.chkResidential.Checked)
            xmlWriter.WriteElementString("Residential", "1");
        else
            xmlWriter.WriteElementString("Residential", "0");
        xmlWriter.WriteEndElement();
        xmlWriter.WriteEndElement();
        xmlWriter.WriteStartElement("Origin");
        xmlWriter.WriteStartElement("Address");
        xmlWriter.WriteElementString("StreetLines", TLS_waybill.paddress1);
        xmlWriter.WriteElementString("City", TLS_waybill.pcity);
        xmlWriter.WriteElementString("StateOrProvinceCode", TLS_waybill.pstate);
        xmlWriter.WriteElementString("PostalCode", TLS_waybill.pzip);
        xmlWriter.WriteElementString("CountryCode", TLS_waybill.pcountry);
        xmlWriter.WriteEndElement();
        xmlWriter.WriteEndElement();
        xmlWriter.WriteStartElement("SpecialServicesRequested");
        xmlWriter.WriteStartElement("ShipmentDryIceDetail");
        xmlWriter.WriteElementString("PackageCount", "1");
        xmlWriter.WriteStartElement("TotalWeight");
        xmlWriter.WriteElementString("Units", TLS_waybill.c_unit.ToString());
        xmlWriter.WriteElementString("Value", TLS_waybill.c_weight.ToString());
        xmlWriter.WriteEndElement();
        xmlWriter.WriteEndElement();
        xmlWriter.WriteEndElement();
        xmlWriter.WriteElementString("PackageCount", dataTable.Rows.Count.ToString());
        if (dataTable.Rows.Count > 0)
        {
            foreach (DataRow row in (InternalDataCollectionBase)dataTable.Rows)
            {
                if ((row["skids"] != DBNull.Value ? row["skids"].ToString().Trim() : "0") == "0")
                {
                    string str1 = row["pieces"] != DBNull.Value ? row["pieces"].ToString().Trim() : "0";
                }
                string str2 = row["length"] != DBNull.Value ? row["length"].ToString().Trim() : "0";
                string str3 = row["height"] != DBNull.Value ? row["height"].ToString().Trim() : "0";
                string str4 = row["width"] != DBNull.Value ? row["width"].ToString().Trim() : "0";
                string str5 = row["weight"] != DBNull.Value ? row["weight"].ToString().Trim() : "0";
                xmlWriter.WriteStartElement("RequestedPackageLineItems");
                xmlWriter.WriteElementString("SequenceNumber", "1");
                xmlWriter.WriteElementString("GroupPackageCount", "1");
                xmlWriter.WriteStartElement("Weight");
                xmlWriter.WriteElementString("Units", TLS_waybill.c_unit.ToString());
                xmlWriter.WriteElementString("Value", str5);
                xmlWriter.WriteEndElement();
                xmlWriter.WriteStartElement("Dimensions");
                xmlWriter.WriteElementString("Length", str2);
                xmlWriter.WriteElementString("Width", str4);
                xmlWriter.WriteElementString("Height", str3);
                xmlWriter.WriteElementString("Units", "IN");
                xmlWriter.WriteEndElement();
                if (this.radioSignature.Checked)
                {
                    xmlWriter.WriteStartElement("SpecialServicesRequested");
                    xmlWriter.WriteStartElement("SignatureOptionDetail");
                    xmlWriter.WriteElementString("OptionType", "DIRECT");
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndElement();
                }
                else if (this.radioAdultSignature.Checked)
                {
                    xmlWriter.WriteStartElement("SpecialServicesRequested");
                    xmlWriter.WriteStartElement("SignatureOptionDetail");
                    xmlWriter.WriteElementString("OptionType", "ADULT");
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndElement();
                }
                xmlWriter.WriteEndElement();
            }
        }
        xmlWriter.WriteEndElement();
        xmlWriter.Flush();
    }
    string str6 = string.Empty;
    if (System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\logs\\request_" + (object)num1 + ".xml"))
        str6 = System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "\\logs\\request_" + (object)num1 + ".xml");
    System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "\\logs\\request_" + (object)num1 + ".xml", "<SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:SOAP-ENC=\"http://schemas.xmlsoap.org/soap/encoding/\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:m0=\"http://fedex.com/ws/rate/v24\"><SOAP-ENV:Body>" + str6);
    System.IO.File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "\\logs\\request_" + (object)num1 + ".xml", "</SOAP-ENV:Body></SOAP-ENV:Envelope>");
    string contents = Form_Waybill.HTTPSPOST(url, AppDomain.CurrentDomain.BaseDirectory + "\\logs\\request_" + (object)num1 + ".xml");
    System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "\\logs\\response_" + TLS_waybill.waybill + "_" + (object)num1 + ".xml", contents);
    if (contents.Contains("SOAP-ENV:Fault") || contents.Contains("<HighestSeverity>ERROR</HighestSeverity>"))
    {
        int num2 = (int)MessageBox.Show("Error: " + contents);
    }
    else
    {
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.Load(AppDomain.CurrentDomain.BaseDirectory + "\\logs\\response_" + TLS_waybill.waybill + "_" + (object)num1 + ".xml");
        XmlNodeList elementsByTagName = xmlDocument.GetElementsByTagName("RateReplyDetails");
        int index = 0;
        while (true)
        {
            if (!elementsByTagName[index].InnerText.Contains(this.txtServiceType.Text))
                ++index;
            else
                break;
        }
        string str1 = xmlDocument.GetElementsByTagName("TotalNetChargeWithDutiesAndTaxes")[index].InnerXml.Split('<')[3].Split('>')[1];
        int num3 = (int)MessageBox.Show(this.txtServiceType.Text + " - " + str1);
        SqlConnection connection = new SqlConnection(TLS_config.iConnectionString);
        connection.Open();
        SqlCommand sqlCommand = new SqlCommand("UPDATE [TLSNET_GAPP].[dbo].[tlshcge02] SET custno='FED01', status='OPEN', charge=@charge, sub=@charge, ctotal=@charge WHERE type='PE' AND waybill=@waybill", connection);
        sqlCommand.Parameters.AddWithValue("waybill", (object)TLS_waybill.waybill);
        sqlCommand.Parameters.AddWithValue("charge", (object)Convert.ToDecimal(str1));
        sqlCommand.ExecuteNonQuery();
        connection.Close();
        int num4 = num1 + 1;
    }
}

private static string HTTPSPOST(string url, string payload)
{
    ServicePointManager.Expect100Continue = true;
    ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
    HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
    byte[] buffer = System.IO.File.ReadAllBytes(payload);
    httpWebRequest.ContentType = "text/xml";
    httpWebRequest.ContentLength = (long)buffer.Length;
    httpWebRequest.Method = "POST";
    httpWebRequest.ServicePoint.Expect100Continue = false;
    Stream requestStream = httpWebRequest.GetRequestStream();
    requestStream.Write(buffer, 0, buffer.Length);
    requestStream.Close();
    HttpWebResponse response;
    try
    {
        response = (HttpWebResponse)httpWebRequest.GetResponse();
    }
    catch (WebException ex)
    {
        response = ex.Response as HttpWebResponse;
    }
    return new StreamReader(response.GetResponseStream()).ReadToEnd();
}

private void Form_Waybill_Load(object sender, EventArgs e)
{
    /*
    SqlConnection connection = new SqlConnection(TLS_config.iConnectionString);
    connection.Open();
    SqlCommand commandShipmentExists = new SqlCommand("SELECT Count(*) FROM [TLSNET_GAPP].[dbo].[tlshcgc02] WHERE tracking IS NOT NULL AND waybill = @waybill", connection);
    commandShipmentExists.Parameters.AddWithValue("@waybill", TLS_waybill.waybill);
    int count = (int)commandShipmentExists.ExecuteScalar();
    MessageBox.Show(count.ToString());
    if (count != 0) //Labels have been created
    {
        lblShipStatus.Text = "Shipping Labels Created";
        btnFedexRate.Enabled = false;
        btnFedexShip.Enabled = false;
    }
    else //Labels have NOT been created
    {
        btnFedexDelete.Enabled = false;
    }


    connection.Close();
    connection.Dispose();
    */
}

private void btnFedexShip_Click(object sender, EventArgs e)
{
    //MessageBox.Show("1@" + TLS_waybill.paphone + "2@" + TLS_waybill.baphone + "3@" + TLS_waybill.bphone + "4@" +TLS_waybill.haphone + "5@" + TLS_waybill.hphone + "6@" + TLS_waybill.pphone + "7@" + TLS_waybill.saphone + "8@" + TLS_waybill.sphone);

    //string uncprinter = "\\\\gapp2-pcc\\Zebra ZP 500 (ZPL)";
    //string uncprinter = "\\\\GAPPDesktop-SUE\\Zebra ZP 500 (ZPL)";
    string uncprinter = "\\\\GAPP-Print\\ZebraZP505";

    bool flag = false;
    int num1 = 1;
    string str1 = (string)null;
    Convert.ToInt32(TLS_waybill.c_pieces);
    //string url = "https://wsbeta.fedex.com:443/web-services"; //test
    string url = "https://ws.fedex.com:443/web-services"; //prod
    DataTable dataTable = TLS.Data_Waybill_Cube(TLS.g_waybill, "SC");
    if (dataTable.Rows.Count <= 0)
        return;
    Decimal num2 = new Decimal();
    foreach (DataRow row in (InternalDataCollectionBase)dataTable.Rows)
    {
        if (num1 >= 2)
            flag = true;
        if ((row["skids"] != DBNull.Value ? row["skids"].ToString().Trim() : "0") == "0")
        {
            string str2 = row["pieces"] != DBNull.Value ? row["pieces"].ToString().Trim() : "0";
        }
        string str3 = row["length"] != DBNull.Value ? row["length"].ToString().Trim() : "0";
        string str4 = row["height"] != DBNull.Value ? row["height"].ToString().Trim() : "0";
        string str5 = row["width"] != DBNull.Value ? row["width"].ToString().Trim() : "0";
        string str6 = row["weight"] != DBNull.Value ? row["weight"].ToString().Trim() : "0";
        XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
        using (XmlWriter xmlWriter1 = XmlWriter.Create(AppDomain.CurrentDomain.BaseDirectory + "\\logs\\request_" + (object)num1 + ".xml", new XmlWriterSettings()
        {
            ConformanceLevel = ConformanceLevel.Fragment,
            OmitXmlDeclaration = true
        }))
        {
            xmlWriter1.WriteStartElement("ProcessShipmentRequest", "http://fedex.com/ws/ship/v23");
            xmlWriter1.WriteAttributeString("xmlns", "http://fedex.com/ws/ship/v23");
            xmlWriter1.WriteStartElement("WebAuthenticationDetail");
            xmlWriter1.WriteStartElement("UserCredential");
            //xmlWriter1.WriteElementString("Key", ""); TEST
            //xmlWriter1.WriteElementString("Password", ""); TEST
            xmlWriter1.WriteElementString("Key", ""); //PROD
            xmlWriter1.WriteElementString("Password", ""); //PROD
            xmlWriter1.WriteEndElement();
            xmlWriter1.WriteEndElement();
            xmlWriter1.WriteStartElement("ClientDetail");
            //xmlWriter1.WriteElementString("AccountNumber", ""); //TEST
            //xmlWriter1.WriteElementString("MeterNumber", "");//TEST
            xmlWriter1.WriteElementString("AccountNumber", "");//PROD
            xmlWriter1.WriteElementString("MeterNumber", "");//PROD
            xmlWriter1.WriteEndElement();
            xmlWriter1.WriteStartElement("TransactionDetail");
            xmlWriter1.WriteElementString("CustomerTransactionId", TLS_waybill.waybill);
            xmlWriter1.WriteEndElement();
            xmlWriter1.WriteStartElement("Version");
            xmlWriter1.WriteElementString("ServiceId", "ship");
            xmlWriter1.WriteElementString("Major", "23");
            xmlWriter1.WriteElementString("Intermediate", "0");
            xmlWriter1.WriteElementString("Minor", "0");
            xmlWriter1.WriteEndElement();
            xmlWriter1.WriteStartElement("RequestedShipment");
            xmlWriter1.WriteElementString("ShipTimestamp", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"));
            xmlWriter1.WriteElementString("DropoffType", "REGULAR_PICKUP");
            xmlWriter1.WriteElementString("ServiceType", this.txtServiceType.Text);
            xmlWriter1.WriteElementString("PackagingType", "YOUR_PACKAGING");
            xmlWriter1.WriteElementString("PreferredCurrency", "CAD");
            xmlWriter1.WriteStartElement("Shipper");
            xmlWriter1.WriteStartElement("Contact");
            xmlWriter1.WriteElementString("PersonName", TLS_waybill.pcontact);
            xmlWriter1.WriteElementString("CompanyName", TLS_waybill.pcompany);
            if (!string.IsNullOrEmpty(TLS_waybill.pphone))
                xmlWriter1.WriteElementString("PhoneNumber", TLS_waybill.pphone);
            else if (!string.IsNullOrEmpty(TLS_waybill.paphone))
                xmlWriter1.WriteElementString("PhoneNumber", TLS_waybill.paphone); //used to be .paphone
            //xmlWriter1.WriteElementString("EMailAddress", "stefon@gappexpress.com");
            xmlWriter1.WriteEndElement();
            xmlWriter1.WriteStartElement("Address");
            xmlWriter1.WriteElementString("StreetLines", TLS_waybill.paddress1);
            xmlWriter1.WriteElementString("StreetLines", TLS_waybill.paddress2);
            xmlWriter1.WriteElementString("City", TLS_waybill.pcity);
            xmlWriter1.WriteElementString("StateOrProvinceCode", TLS_waybill.pstate);
            xmlWriter1.WriteElementString("PostalCode", TLS_waybill.pzip);
            xmlWriter1.WriteElementString("CountryCode", TLS_waybill.pcountry);
            xmlWriter1.WriteEndElement();
            xmlWriter1.WriteEndElement();
            xmlWriter1.WriteStartElement("Recipient");
            xmlWriter1.WriteStartElement("Contact");
            xmlWriter1.WriteElementString("PersonName", TLS_waybill.scontact);
            xmlWriter1.WriteElementString("CompanyName", TLS_waybill.scompany);
            if (!string.IsNullOrEmpty(TLS_waybill.sphone))
               xmlWriter1.WriteElementString("PhoneNumber", TLS_waybill.sphone);
            else if (!string.IsNullOrEmpty(TLS_waybill.saphone))
                xmlWriter1.WriteElementString("PhoneNumber", TLS_waybill.saphone); //used to be .saphone
            //xmlWriter1.WriteElementString("EMailAddress", "stefon@gappexpress.com");
            xmlWriter1.WriteEndElement();
            xmlWriter1.WriteStartElement("Address");
            xmlWriter1.WriteElementString("StreetLines", TLS_waybill.saddress1);
            xmlWriter1.WriteElementString("StreetLines", TLS_waybill.saddress2);
            xmlWriter1.WriteElementString("City", TLS_waybill.scity);
            xmlWriter1.WriteElementString("StateOrProvinceCode", TLS_waybill.sstate);
            xmlWriter1.WriteElementString("PostalCode", TLS_waybill.szip);
            xmlWriter1.WriteElementString("CountryCode", TLS_waybill.scountry);
            if (this.chkResidential.Checked)
                xmlWriter1.WriteElementString("Residential", "1");
            else
                xmlWriter1.WriteElementString("Residential", "0");
            xmlWriter1.WriteEndElement();
            xmlWriter1.WriteEndElement();
            xmlWriter1.WriteStartElement("ShippingChargesPayment");
            xmlWriter1.WriteElementString("PaymentType", "SENDER");
            xmlWriter1.WriteStartElement("Payor");
            xmlWriter1.WriteStartElement("ResponsibleParty");
            xmlWriter1.WriteElementString("AccountNumber", "888155716");
            xmlWriter1.WriteStartElement("Tins");
            xmlWriter1.WriteElementString("TinType", "BUSINESS_STATE");
            xmlWriter1.WriteElementString("Number", "213456");
            xmlWriter1.WriteEndElement();
            xmlWriter1.WriteStartElement("Contact");
            xmlWriter1.WriteElementString("ContactId", "12345");
            xmlWriter1.WriteElementString("PersonName", "Pooja Bali");
            xmlWriter1.WriteEndElement();
            xmlWriter1.WriteEndElement();
            xmlWriter1.WriteEndElement();
            xmlWriter1.WriteEndElement();
            xmlWriter1.WriteStartElement("CustomsClearanceDetail");
            xmlWriter1.WriteStartElement("DutiesPayment");
            xmlWriter1.WriteElementString("PaymentType", "SENDER");
            xmlWriter1.WriteStartElement("Payor");
            xmlWriter1.WriteStartElement("ResponsibleParty");
            xmlWriter1.WriteElementString("AccountNumber", "510087100");
            xmlWriter1.WriteStartElement("Tins");
            xmlWriter1.WriteElementString("TinType", "BUSINESS_STATE");
            xmlWriter1.WriteElementString("Number", "213456");
            xmlWriter1.WriteEndElement();
            xmlWriter1.WriteStartElement("Contact");
            xmlWriter1.WriteElementString("ContactId", "12345");
            xmlWriter1.WriteElementString("PersonName", "");
            xmlWriter1.WriteEndElement();
            xmlWriter1.WriteEndElement();
            xmlWriter1.WriteEndElement();
            xmlWriter1.WriteEndElement();
            xmlWriter1.WriteElementString("DocumentContent", "DOCUMENTS_ONLY");
            xmlWriter1.WriteStartElement("CustomsValue");
            xmlWriter1.WriteElementString("Currency", "CAD");
            XmlWriter xmlWriter2 = xmlWriter1;
            Decimal cCusvalue = TLS_waybill.c_cusvalue;
            string str7 = cCusvalue.ToString();
            xmlWriter2.WriteElementString("Amount", str7);
            xmlWriter1.WriteEndElement();
            xmlWriter1.WriteStartElement("Commodities");
            xmlWriter1.WriteElementString("NumberOfPieces", "1");
            xmlWriter1.WriteElementString("Description", TLS_waybill.c_descr);
            xmlWriter1.WriteElementString("CountryOfManufacture", "US");
            xmlWriter1.WriteStartElement("Weight");
            xmlWriter1.WriteElementString("Units", TLS_waybill.c_unit.ToString());
            xmlWriter1.WriteElementString("Value", TLS_waybill.c_weight.ToString());
            xmlWriter1.WriteEndElement();
            xmlWriter1.WriteElementString("Quantity", "1");
            xmlWriter1.WriteElementString("QuantityUnits", "cm");
            xmlWriter1.WriteStartElement("UnitPrice");
            xmlWriter1.WriteElementString("Currency", "CAD");
            xmlWriter1.WriteElementString("Amount", "1");
            xmlWriter1.WriteEndElement();
            xmlWriter1.WriteStartElement("CustomsValue");
            xmlWriter1.WriteElementString("Currency", "CAD");
            XmlWriter xmlWriter3 = xmlWriter1;
            cCusvalue = TLS_waybill.c_cusvalue;
            string str8 = cCusvalue.ToString();
            xmlWriter3.WriteElementString("Amount", str8);
            xmlWriter1.WriteEndElement();
            xmlWriter1.WriteEndElement();
            xmlWriter1.WriteEndElement();
            xmlWriter1.WriteStartElement("LabelSpecification");
            xmlWriter1.WriteElementString("LabelFormatType", "COMMON2D");
            xmlWriter1.WriteElementString("ImageType", "ZPLII");
            xmlWriter1.WriteElementString("LabelStockType", "STOCK_4X6.75_LEADING_DOC_TAB");
            xmlWriter1.WriteElementString("LabelPrintingOrientation", "TOP_EDGE_OF_TEXT_FIRST");
            xmlWriter1.WriteStartElement("CustomerSpecifiedDetail");
            xmlWriter1.WriteStartElement("DocTabContent");
            xmlWriter1.WriteElementString("DocTabContentType", "ZONE001");
            xmlWriter1.WriteStartElement("Zone001");
            xmlWriter1.WriteStartElement("DocTabZoneSpecifications");
            xmlWriter1.WriteElementString("ZoneNumber", "1");
            xmlWriter1.WriteElementString("Header", "REF");
            xmlWriter1.WriteElementString("DataField", "REQUEST/PACKAGE/CustomerReferences[CustomerReferenceType=\"CUSTOMER_REFERENCE\"]/value");
            xmlWriter1.WriteElementString("Justification", "LEFT");
            xmlWriter1.WriteEndElement();
            xmlWriter1.WriteStartElement("DocTabZoneSpecifications");
            xmlWriter1.WriteElementString("ZoneNumber", "2");
            xmlWriter1.WriteElementString("Header", "SHP");
            xmlWriter1.WriteElementString("DataField", "REQUEST/SHIPMENT/ShipTimestamp");
            xmlWriter1.WriteElementString("Justification", "LEFT");
            xmlWriter1.WriteEndElement();
            xmlWriter1.WriteStartElement("DocTabZoneSpecifications");
            xmlWriter1.WriteElementString("ZoneNumber", "3");
            xmlWriter1.WriteElementString("Header", "PON");
            xmlWriter1.WriteElementString("DataField", "REQUEST/PACKAGE/CustomerReferences[CustomerReferenceType=\"P_O_NUMBER\"]/value");
            xmlWriter1.WriteElementString("Justification", "LEFT");
            xmlWriter1.WriteEndElement();
            xmlWriter1.WriteStartElement("DocTabZoneSpecifications");
            xmlWriter1.WriteElementString("ZoneNumber", "4");
            xmlWriter1.WriteElementString("Header", "WHT");
            xmlWriter1.WriteElementString("DataField", "REQUEST/PACKAGE/weight/Value");
            xmlWriter1.WriteElementString("Justification", "LEFT");
            xmlWriter1.WriteEndElement();
            xmlWriter1.WriteStartElement("DocTabZoneSpecifications");
            xmlWriter1.WriteElementString("ZoneNumber", "5");
            xmlWriter1.WriteElementString("Header", "BASE");
            xmlWriter1.WriteElementString("DataField", "REPLY/SHIPMENT/RATES/PAYOR_ACCOUNT_PACKAGE/TotalBaseCharge/Amount");
            xmlWriter1.WriteElementString("Justification", "LEFT");
            xmlWriter1.WriteEndElement();
            xmlWriter1.WriteStartElement("DocTabZoneSpecifications");
            xmlWriter1.WriteElementString("ZoneNumber", "6");
            xmlWriter1.WriteElementString("Header", "SURCHG");
            xmlWriter1.WriteElementString("DataField", "REPLY/SHIPMENT/RATES/PAYOR_ACCOUNT_PACKAGE/TotalSurcharges/Amount");
            xmlWriter1.WriteElementString("Justification", "LEFT");
            xmlWriter1.WriteEndElement();
            xmlWriter1.WriteStartElement("DocTabZoneSpecifications");
            xmlWriter1.WriteElementString("ZoneNumber", "7");
            xmlWriter1.WriteElementString("Header", "REF");
            xmlWriter1.WriteElementString("DataField", "REQUEST/PACKAGE/CustomerReferences[CustomerReferenceType=\"CUSTOMER_REFERENCE\"]/value");
            xmlWriter1.WriteElementString("Justification", "LEFT");
            xmlWriter1.WriteEndElement();
            xmlWriter1.WriteStartElement("DocTabZoneSpecifications");
            xmlWriter1.WriteElementString("ZoneNumber", "8");
            xmlWriter1.WriteElementString("Header", "NETCHG");
            xmlWriter1.WriteElementString("DataField", "REPLY/SHIPMENT/RATES/PAYOR_ACCOUNT_PACKAGE/TotalNetFedExCharge/Amount");
            xmlWriter1.WriteElementString("Justification", "LEFT");
            xmlWriter1.WriteEndElement();
            xmlWriter1.WriteEndElement();
            xmlWriter1.WriteEndElement();
            xmlWriter1.WriteEndElement();
            xmlWriter1.WriteEndElement();
            xmlWriter1.WriteElementString("RateRequestTypes", "LIST");
            if (flag)
            {
                xmlWriter1.WriteStartElement("MasterTrackingId");
                xmlWriter1.WriteElementString("TrackingIdType", "FEDEX");
                xmlWriter1.WriteElementString("FormId", "0488");
                xmlWriter1.WriteElementString("TrackingNumber", str1);
                xmlWriter1.WriteEndElement();
            }
            xmlWriter1.WriteElementString("PackageCount", TLS_waybill.c_pieces.ToString());
            xmlWriter1.WriteStartElement("RequestedPackageLineItems");
            xmlWriter1.WriteElementString("SequenceNumber", num1.ToString());
            xmlWriter1.WriteStartElement("Weight");
            xmlWriter1.WriteElementString("Units", "LB");
            xmlWriter1.WriteElementString("Value", str6);
            xmlWriter1.WriteEndElement();
            xmlWriter1.WriteStartElement("Dimensions");
            xmlWriter1.WriteElementString("Length", str3);
            xmlWriter1.WriteElementString("Width", str5);
            xmlWriter1.WriteElementString("Height", str4);
            xmlWriter1.WriteElementString("Units", "IN");
            xmlWriter1.WriteEndElement();
            xmlWriter1.WriteStartElement("CustomerReferences");
            xmlWriter1.WriteElementString("CustomerReferenceType", "CUSTOMER_REFERENCE");
            xmlWriter1.WriteElementString("Value", TLS_waybill.waybill);
            xmlWriter1.WriteEndElement();
            xmlWriter1.WriteStartElement("CustomerReferences");
            xmlWriter1.WriteElementString("CustomerReferenceType", "P_O_NUMBER");
            xmlWriter1.WriteElementString("Value", TLS_waybill.reference);
            xmlWriter1.WriteEndElement();
            if (this.radioSignature.Checked)
            {
                xmlWriter1.WriteStartElement("SpecialServicesRequested");
                xmlWriter1.WriteStartElement("SignatureOptionDetail");
                xmlWriter1.WriteElementString("OptionType", "DIRECT");
                xmlWriter1.WriteEndElement();
                xmlWriter1.WriteEndElement();
            }
            else if (this.radioAdultSignature.Checked)
            {
                xmlWriter1.WriteStartElement("SpecialServicesRequested");
                xmlWriter1.WriteStartElement("SignatureOptionDetail");
                xmlWriter1.WriteElementString("OptionType", "ADULT");
                xmlWriter1.WriteEndElement();
                xmlWriter1.WriteEndElement();
            }
            xmlWriter1.WriteEndElement();
            xmlWriter1.WriteEndElement();
            xmlWriter1.Flush();
        }
        string str9 = string.Empty;
        if (System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\logs\\request_" + (object)num1 + ".xml"))
            str9 = System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "\\logs\\request_" + (object)num1 + ".xml");
        System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "\\logs\\request_" + (object)num1 + ".xml", "<SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:SOAP-ENC=\"http://schemas.xmlsoap.org/soap/encoding/\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:m0=\"http://fedex.com/ws/ship/v23\"><SOAP-ENV:Body>" + str9);
        System.IO.File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "\\logs\\request_" + (object)num1 + ".xml", "</SOAP-ENV:Body></SOAP-ENV:Envelope>");
        string contents = Form_Waybill.HTTPSPOST(url, AppDomain.CurrentDomain.BaseDirectory + "\\logs\\request_" + (object)num1 + ".xml");
        System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "\\logs\\response_" + TLS_waybill.waybill + "_" + (object)num1 + ".xml", contents);
        if (contents.Contains("SOAP-ENV:Fault") || contents.Contains("<HighestSeverity>ERROR</HighestSeverity>"))
        {
            int num3 = (int)MessageBox.Show("Error: " + contents);
        }
        else
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(AppDomain.CurrentDomain.BaseDirectory + "\\logs\\response_" + TLS_waybill.waybill + "_" + (object)num1 + ".xml");
            string str7 = (string)null;
            XmlNodeList elementsByTagName1 = xmlDocument.GetElementsByTagName("TrackingNumber");
            for (int index = 0; index < elementsByTagName1.Count; ++index)
            {
                if (num1 == 1)
                {
                    str1 = elementsByTagName1[index].InnerXml;
                    str7 = elementsByTagName1[index].InnerXml;
                    break;
                }
                str7 = elementsByTagName1[index].InnerXml;
            }
            int num4 = 0;
            SqlConnection connection = new SqlConnection(TLS_config.iConnectionString);
            connection.Open();
            SqlCommand sqlCommand1 = new SqlCommand("WITH myTableWithRows AS (SELECT (ROW_NUMBER() OVER (ORDER BY [TLSNET_GAPP].[dbo].[tlshcgc02].waybill_id)) as row,* FROM [TLSNET_GAPP].[dbo].[tlshcgc02] WHERE waybill = @waybill) SELECT * FROM myTableWithRows WHERE row = @round", connection);
            sqlCommand1.Parameters.AddWithValue("round", (object)num1);
            sqlCommand1.Parameters.AddWithValue("waybill", (object)TLS_waybill.waybill);
            SqlDataReader sqlDataReader = sqlCommand1.ExecuteReader();
            while (sqlDataReader.Read())
                num4 = Convert.ToInt32(sqlDataReader["waybill_id"].ToString());
            sqlDataReader.Close();
            int num5 = (int)MessageBox.Show("Waybill ID " + (object)num4 + " to be assigned tracking " + str7);
            SqlCommand sqlCommand2 = new SqlCommand("UPDATE [TLSNET_GAPP].[dbo].[tlshcgc02] SET tracking=@tracking WHERE waybill_id=@waybill_id", connection);
            sqlCommand2.Parameters.AddWithValue("tracking", (object)str7);
            sqlCommand2.Parameters.AddWithValue("waybill_id", (object)num4);
            sqlCommand2.ExecuteNonQuery();

            string text;
            try
            {
                text = xmlDocument.GetElementsByTagName("TotalNetChargeWithDutiesAndTaxes")[0].InnerXml.Split('<')[3].Split('>')[1];
            }
            catch(Exception ex)
            {
                text = xmlDocument.GetElementsByTagName("NetCharge")[0].InnerXml.Split('<')[3].Split('>')[1];
            }


            //string text = xmlDocument.GetElementsByTagName("NetCharge")[0].InnerXml.Split('<')[3].Split('>')[1];
            int num6 = (int)MessageBox.Show(text);
            num2 += Convert.ToDecimal(text);
            XmlNodeList elementsByTagName2 = xmlDocument.GetElementsByTagName("Image");
            for (int index = 0; index < elementsByTagName2.Count; ++index)
            {
                /*
                if (System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\" + TLS_waybill.waybill + "_" + (object)num1 + ".png"))
                    System.IO.File.Delete(AppDomain.CurrentDomain.BaseDirectory + "\\" + TLS_waybill.waybill + "_" + (object)num1 + ".png");
                byte[] buffer = Convert.FromBase64String(elementsByTagName2[index].InnerXml);
                BinaryWriter binaryWriter = new BinaryWriter((Stream)new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\" + TLS_waybill.waybill + "_" + (object)num1 + ".png", FileMode.CreateNew));
                binaryWriter.Write(buffer, 0, buffer.Length);
                binaryWriter.Close();
                //Process.Start(AppDomain.CurrentDomain.BaseDirectory + "\\" + TLS_waybill.waybill + "_" + (object)num1 + ".png");
                */
                //Show Base64 code
                MessageBox.Show(elementsByTagName2[index].InnerXml);

                //Convert Base64 to ZPL
                byte[] zplBytes = System.Convert.FromBase64String(elementsByTagName2[index].InnerXml);
                string zplString = System.Text.ASCIIEncoding.ASCII.GetString(zplBytes);

                //Send ZPL code to UNC printer share
                RawPrinterHelper.SendStringToPrinter(uncprinter, zplString);
            }
            ++num1;
        }
    }
    int num7 = (int)MessageBox.Show("Loop K over - " + num2.ToString());
    SqlConnection connection1 = new SqlConnection(TLS_config.iConnectionString);
    connection1.Open();
    SqlCommand sqlCommand = new SqlCommand("UPDATE [TLSNET_GAPP].[dbo].[tlshcge02] SET custno='FED01', status='OPEN', charge=@charge, sub=@charge, ctotal=@charge WHERE type='PE' AND waybill=@waybill", connection1);
    sqlCommand.Parameters.AddWithValue("waybill", (object)TLS_waybill.waybill);
    sqlCommand.Parameters.AddWithValue("charge", (object)Convert.ToDecimal(num2));
    sqlCommand.ExecuteNonQuery();
    connection1.Close();

}

  private void btnFedexDelete_Click(object sender, EventArgs e)
  {          
      ///string url = "https://wsbeta.fedex.com:443/web-services"; //test
      string url = "https://ws.fedex.com:443/web-services"; //prod
      string TrackingToDelete = null;
      int round = 0;

      SqlConnection connection = new SqlConnection(TLS_config.iConnectionString);

      try
      {
          connection.Open();
          SqlCommand warehouses = new SqlCommand("SELECT waybill_id, waybill, tracking FROM [TLSNET_GAPP].[dbo].[tlshcgc02] WHERE waybill = @waybill AND tracking IS NOT NULL;", connection);
          warehouses.Parameters.AddWithValue("@waybill", g_waybill); //g_waybill may cause error. Investigate this first.
          SqlDataReader rdr = warehouses.ExecuteReader();
          while (rdr.Read()) //Loop for each Tracking Number (per package)
          {
              round += 1;
              TrackingToDelete = rdr["tracking"].ToString();

              MessageBox.Show(g_waybill +" - " + TrackingToDelete + " - " + round);

              XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
              using (XmlWriter xmlWriter1 = XmlWriter.Create(AppDomain.CurrentDomain.BaseDirectory + "\\logs\\request_DEL" + TLS_waybill.waybill + "-" +round+".xml", new XmlWriterSettings()
              {
                  ConformanceLevel = ConformanceLevel.Fragment,
                  OmitXmlDeclaration = true
              }))
              {
                  xmlWriter1.WriteStartElement("DeleteShipmentRequest", "http://fedex.com/ws/ship/v17");
                  //xmlWriter1.WriteAttributeString("xmlns", "http://fedex.com/ws/ship/v17");
                      xmlWriter1.WriteStartElement("WebAuthenticationDetail");
                          xmlWriter1.WriteStartElement("UserCredential");
                              //xmlWriter1.WriteElementString("Key", ""); //test
                              //xmlWriter1.WriteElementString("Password", ""); //test
                              xmlWriter1.WriteElementString("Key", ""); //PROD
                              xmlWriter1.WriteElementString("Password", ""); //PROD
                  xmlWriter1.WriteEndElement();
                      xmlWriter1.WriteEndElement();
                      xmlWriter1.WriteStartElement("ClientDetail");
                          //xmlWriter1.WriteElementString("AccountNumber", ""); //test
                          //xmlWriter1.WriteElementString("MeterNumber", ""); //test
                          xmlWriter1.WriteElementString("AccountNumber", "");//PROD
                          xmlWriter1.WriteElementString("MeterNumber", "");//PROD
                  xmlWriter1.WriteEndElement();
                      xmlWriter1.WriteStartElement("TransactionDetail");
                          xmlWriter1.WriteElementString("CustomerTransactionId", "Delete Shipment");
                      xmlWriter1.WriteEndElement();
                      xmlWriter1.WriteStartElement("Version");
                          xmlWriter1.WriteElementString("ServiceId", "ship");
                          xmlWriter1.WriteElementString("Major", "17");
                          xmlWriter1.WriteElementString("Intermediate", "0");
                          xmlWriter1.WriteElementString("Minor", "0");
                      xmlWriter1.WriteEndElement();
                      xmlWriter1.WriteStartElement("TrackingId");
                          xmlWriter1.WriteElementString("TrackingIdType", "EXPRESS");
                          xmlWriter1.WriteElementString("FormId", "0201");
                          xmlWriter1.WriteElementString("TrackingNumber", TrackingToDelete);
                      xmlWriter1.WriteEndElement();
                      xmlWriter1.WriteElementString("DeletionControl", "DELETE_ALL_PACKAGES");
                  xmlWriter1.WriteEndElement();

              }

              MessageBox.Show(TLS_waybill.waybill);

              //Add Soap Envelope
              string str9 = string.Empty;
              if (System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\logs\\request_DEL" + TLS_waybill.waybill + "-" + round + ".xml"))
                  str9 = System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "\\logs\\request_DEL" + TLS_waybill.waybill + "-" + round + ".xml");
              System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "\\logs\\request_DEL" + TLS_waybill.waybill + "-" + round + ".xml", "<SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:v17=\"http://fedex.com/ws/ship/v17\"><SOAP-ENV:Body>" + str9);
              System.IO.File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "\\logs\\request_DEL" + TLS_waybill.waybill + "-" + round + ".xml", "</SOAP-ENV:Body></SOAP-ENV:Envelope>");

              string contents = Form_Waybill.HTTPSPOST(url, AppDomain.CurrentDomain.BaseDirectory + "\\logs\\request_DEL" + TLS_waybill.waybill + "-" +round+ ".xml");

              System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "\\logs\\response_DEL" + TLS_waybill.waybill + "_" + round + ".xml", contents);
              if (contents.Contains("SOAP-ENV:Fault") || contents.Contains("<HighestSeverity>ERROR</HighestSeverity>"))
              {
                  MessageBox.Show("Error: " + contents);
              }
              else //No error so okay to delete tracking from our database
              {
                  SqlConnection connection1 = new SqlConnection(TLS_config.iConnectionString);
                  connection1.Open();
                  SqlCommand sqlCommand = new SqlCommand("UPDATE [TLSNET_GAPP].[dbo].[tlshcgc02] SET tracking=NULL WHERE tracking=@tracking", connection1);
                  sqlCommand.Parameters.AddWithValue("tracking", TrackingToDelete);
                  sqlCommand.ExecuteNonQuery();
                  connection1.Close();
              }
          }
          rdr.Close();
      }
      catch (Exception ex)
      {
          MessageBox.Show(ex.Message);
      }
      finally
      {
          connection.Close();
      }



      //Delete rows from Table

      //Enable buttons
  }
