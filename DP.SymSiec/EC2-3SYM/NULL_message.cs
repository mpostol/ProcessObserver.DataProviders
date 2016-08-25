namespace ApplicationLayer.NULL
{
  using System;
  using Processes;
  /// <summary>
  /// Summary description for NULL_message.
  /// </summary>
  public class NULL_message: ProtocolALMessage
  {
    private object[] buffor = new object[100];
    public override object GetValueFromMess(int regAddress)
    {
/*		switch(regAddress)
		{
			case 0:
				buffor[regAddress] = !(bool)buffor[regAddress];//(1000*station + address + idx);
				break;
			case 1:
				buffor[regAddress] = (byte)buffor[regAddress]+1;//(1000*station + address + idx);
				break;
			case 2:
				buffor[regAddress] = (uint)buffor[regAddress]+1;//(1000*station + address + idx);
				break;
			case 3:
				buffor[regAddress] = (int)buffor[regAddress]+1;//(1000*station + address + idx);
				break;
			case 4:
				buffor[regAddress] = (long)buffor[regAddress]+1;//(1000*station + address + idx);
				break;
			case 5:
				buffor[regAddress] = (float)buffor[regAddress]+1;//(1000*station + address + idx);
				break;
			case 6:
				buffor[regAddress] = (double)buffor[regAddress]+1;//(1000*station + address + idx);
				break;


		}
*/		return buffor[regAddress];
    }
    public override object ReadCMD(int regAddress)
    {
		return buffor[regAddress];
    }
    public override void WriteValue(object regValue, int regAddress)
    {
      buffor[regAddress] = regValue;
    }
    public override void SetValue(object regValue, int regAddress)
    {
      buffor[regAddress] = regValue;
    }
  private static uint teraz=0;
    public override void SetBlockDescription
      (int station, int address, DataType myDataType, int length)
    {
		Random r=new Random();
		teraz=(uint)r.Next(999999);
		uint teraz2=(uint)r.Next(100)+(uint)address;
		//System.Windows.Forms.MessageBox.Show(teraz.ToString());
      base.SetBlockDescription(station, address, myDataType, length);
      int addressdatatype=address%7;
		switch(addressdatatype)
		{
			case 0: //bool
				for (int idx = 0; idx < length; idx++)
				{
					//if(buffor[idx]==null) buffor[idx] = (bool)true;//(1000*station + address + idx);
					//else buffor[idx] = (bool)false;
					if((teraz2%100)<50)buffor[idx] = (bool)false;else buffor[idx] = (bool)true;
				}
				break;
			case 1: //byte
				for (int idx = 0; idx < length; idx++)
				{
					buffor[idx] = (byte)1;//(1000*station + address + idx);
				}
				break;
			case 2: //unsigned integer
				for (int idx = 0; idx < length; idx++)
				{
					if((r.Next(100)%100)<75)
					{
						int bit=r.Next(17);
						uint v=1;
						for(int i=0;i<bit;i++)
						{
							v=v<<1;
						}
						buffor[idx] = v; 

					}
					else buffor[idx] = (uint)0;//(1000*station + address + idx);
				}
				break;
			case 3://integer
				for (int idx = 0; idx < length; idx++)
				{
					if((r.Next(100)%100)<25)
					{
						int bit=r.Next(100)%16;
						int v=1<<bit;
						buffor[idx] = v; 

					}
					else buffor[idx] = (int)0;//(1000*station + address + idx);
					//buffor[idx] = (int)1;
				}
				break;
			case 4:
				for (int idx = 0; idx < length; idx++)
				{
					buffor[idx] = (long)1;//(1000*station + address + idx);
				}
				break;
			case 5://real
				for (int idx = 0; idx < length; idx++)
				{
					//if(buffor[idx]==null) buffor[idx] = (float)1.1;//(1000*station + address + idx);
					//else buffor[idx] = (float)buffor[idx]+1.1;
					float val=1.1F;
					int retries=1000;
					while(retries-->0)
					{
						if(address>=600  && address<700)val=(float)1.1+teraz%999999+(float)((float)teraz2/100.0);
						else if(address>=500  && address<600)val=(float)1.1+teraz%20+(float)((float)teraz2/100.0);
						else if(address>=400  && address<500)val=(float)1.1+teraz%60+(float)((float)teraz2/100.0);
						else val=(float)1.1+teraz%10+(float)((float)teraz2/100.0);
						//exit from the loop when some conditions are true
						if(val>0 && val<999999 && address>600  && address<700) break;
						if(val>2 && val<20 && address>500  && address<600) break;
						if(val>20 && val<60 && address>400  && address<500) break;
						//break;
					}
					buffor[idx] = val;
				}
				break;
			case 6:
				for (int idx = 0; idx < length; idx++)
				{
					buffor[idx] = (double)1.01;//(1000*station + address + idx);
				}
				break;


		}
    }
    public NULL_message( CommunicationLayer.SesDBufferPool homePool):base(30, homePool, false )
    {}
  }
}