//<summary>
//  Title   : Symulator
//  System  : Microsoft Visual C# .NET 2005
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//    Maciej Zbrzezny - 12-04-2006
//    dodano wszystkie tagi by byly dostepne przez OPC, dodano resetowanie itp...
//    MP - 2005 created
//
//  Copyright (C)2006, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using CAS.Lib.RTLib.Processes;

namespace CAS.Lib.CommonBus.ApplicationLayer.EC2_3SYM
{
  /// <summary>
  /// Summary description for Symulator.
  /// 
  ///  Tagi OPC Symulatora:
  ///  Uwaga w OPC Serwerze dane te maj¹ przedrostek S/SYMEC2EC3/
  ///  AKPP – stopieñ otwarcia zasuwy na „powrocie” w komorze podzia³owej. (0 – zamkniêta , 10000 – otwarta) (wartoœæ zadawana z symulatora uruchomionego w sterowniku PLC lub rêcznie )
  ///  AKPZ – stopieñ otwarcia zasuwy na „zasillaniu” w komorze podzia³owej. (0 – zamkniêta , 10000 – otwarta) (wartoœæ zadawana z symulatora uruchomionego w sterowniku PLC lub rêcznie )
  ///  AKRP – stopieñ otwarcia zasuwy na „powrocie” w komorze regulacyjnej. (0 – zamkniêta , 10000 – otwarta) (wartoœæ zadawana z symulatora uruchomionego w sterowniku PLC lub rêcznie )
  ///  AKRZ – stopieñ otwarcia zasuwy na „zasillaniu” w komorze regulacyjnej. (0 – zamkniêta , 10000 – otwarta) (wartoœæ zadawana z symulatora uruchomionego w sterowniku PLC lub rêcznie )
  ///  F2, F3 – przep³yw w t/h w Ec2 i Ec3
  ///  F2u, F3u – przep³yw w t/h uzupe³nieñ w Ec2 i Ec3
  ///  P2, P3 – ciœnienie w kPa w Ec2 i Ec3 (wysokoœæ podnoszenia) (wartoœæ zadawana) (zalecane wartoœci P2: 800 – 900, P3: 780 – 850)
  ///  P2U – ciœnienie na powrocie w EC2 (w kPa)
  ///  P2Z – ciœnienie na zasilaniu w EC2 (w kPa)
  ///  P3U – ciœnienie na powrocie w EC3 (w kPa)
  ///  P3Z – ciœnienie na zasilaniu w EC3 (w kPa)
  ///  P2KPP, P3KPP – Ciœnienia „na powrocie” w komorze podzia³owej na zasuwie po stronie EC2 i EC3 (w kPa)
  ///  P2KPZ, P3KPZ – Ciœnienia „na zasilaniu” w komorze podzia³owej na zasuwie po stronie EC2 i EC3 (w kPa)
  ///  PKRP , PKRZ – Ciœnienia w komorze (w kPa) regulacyjnej (na zasilaniu i powrocie)
  ///  Fus – wspó³czynnik zmian dla uzupe³nieñ (standardowa wartoœæ 5)
  ///  EC2_SUMA_F_WODA, EC3_SUMA_F_WODA – ca³kowite sumy przep³ywów dla EC
  ///  UWAGA – wartoœci podkreœlone – oznaczaj¹ wartoœci, które s¹ generowane przez symulator .
  ///  Tagi Konfiguracyjne OPC Symulatora
  ///  Uwaga: wprowadzane tu zmiany wymagaj¹ najczêœciej reinicjalizacji symulatora – patrz wydawane komendy (komenda Initialise).
  ///  Uwaga w OPC Serwerze dane te maj¹ przedrostek S/SYMEC2EC3/CONF/
  ///  Pu2, Pu3 – Ciœnienia uzupe³nieñ w EC2 i EC3 (w kPa) (standardowa wartoœæ 220)
  ///  Ao, Az – K¹ty dla otwartej i zamkniêtej zasuwy (standardowe wartoœci 10000 i 0)
  ///  Cycle – wartoœæ w milisekundach – pomiêdzy kolejnymi krokami symulatora (standardowa wartoœæ 100)
  ///  F2n, F3n –znamionowe przep³ywy w EC2 i EC3 (standardowe wartoœci 200 i 1500)
  ///  P2kpzn, P2kppn, P3kpzn, P3kppn – Ciœnienia znamionowe  (w kPa) w komorze  podzia³owej na zasilaniu i powrocie, od strony EC2 i EC3 (standardowe wartoœci 820, 140, 1020, 240)
  ///  P2zn, P3zn – Ciœnienia znamionowe (w kPa) na wyjœciach EC2 i EC3 (standardowe wartoœci 1120, 1000)
  ///  P3Stat – ciœnienie statyczne  (w kPa) wynikaj¹ce z ró¿nicy wysokoœci pomiêdzy EC2 i EC3 (standardowa wartoœæ 100)
  ///  Pu (standardowa wartoœæ 220)
  ///  R2p – wspó³czynnik oporu hydraulicznego na powrocie po stronie EC2 (standardowa wartoœæ 0, inna zalecana wartoœæ (Pu - P2kppn)/F2n lub 0,1)
  ///  R2z – wspó³czynnik oporu hydraulicznego po stronie EC2 (standardowa wartoœæ 0, inna zalecana wartoœæ R2p lub 0,3)
  ///  R3p – wspó³czynnik oporu hydraulicznego na powrocie po stronie EC3 (standardowa wartoœæ 0, inna zalecana wartoœæ (Pu + P3Stat - P3kppn)/F3n lub 0,1)
  ///  R3z – wspó³czynnik oporu hydraulicznego po stronie EC3 (standardowa wartoœæ 0, inna zalecana wartoœæ R3p lub 0,2)
  ///  R2o – wspó³czynnik oporu zwi¹zany z obci¹¿eniem po stronie EC2 (standardowa wartoœæ 3.4 =  ( P2kpzn - P2kppn ) / F2n  , aby zwiêkszyæ obci¹¿enie mo¿na j¹ zwiêkszyæ np. 10 razy lub inna zalecana wartoœæ 1,6)
  ///  R3o – wspó³czynnik oporu zwi¹zany z obci¹¿eniem po stronie EC3 (standardowa wartoœæ 0.52 =  ( P3kpzn - P3kppn ) / F3n, aby zwiêkszyæ obci¹¿enie mo¿na j¹ zwiêkszyæ np. 10 razy lub inna zalecana wartoœæ 0,4)
  ///  R3u – wspó³czynnik oporu uzupe³nieñ - standardowa wartoœæ 100/3000=0,033
  ///  Tdz – sta³a czasowa (standardowa wartoœæ: 0.0011= F2n / (P2zn-Pu) /( 1000*20 / cycle) ) 
  ///  Tdu –sta³a czasowa (standardowa wartoœæ: 0.0002=  Tdz / 5.0)
  ///  F2_other_direction, F3_other_direction – wartoœæ przep³ywu w pozosta³ych kierunkach (nie objêtych regulacj¹), wartoœæ u¿ywana do wyznaczenia sumy przep³ywu dla ca³ego EC
  ///  Uwaga: Manipulowanie wspó³czynnikami oporu hydraulicznego odcinków ruroci¹gów pozwala na dobranie w³aœciwych ciœnieñ w komorze podzia³owej I na powrocie EC
  ///  Uwaga: Manipulowanie wspó³czynnikami oporu zwi¹zanymi z obci¹¿eniem (odbiorami) pozwala na dobór w³aœciwej wartoœci przep³ywu, oraz spadku ciœnienia miêdzy zasilaniem a powrotem w komorze podzia³owej)
  ///  Tagi Komend OPC Symulatora
  ///  ResetAll – resetuje symulator do zastaw standardowych
  ///  Initialise – resetuje symulator do nastaw zdefiniowanych w tagach konfiguracyjnych
  ///  Szczegó³y konfiguracyjne
  ///  Wszystkie dane powinny byæ odczytywane jako typ Double. W poni¿szym przyk³adzie konfiguracyjnym u¿yto dla symulatora stacji o adresie 93 i nazwie ST_SYMEC2EC3.
  ///  Odczytywane bloki
  ///  93;1000;10000;1000;10000;0;10;63
  ///  93;1000;10000;1000;10000;0;0;5
  ///  Mapowanie nazw
  /// 
  /// ST_SYMEC2EC3/0/add0;S/SYMEC2EC3/ResetAll
  /// ST_SYMEC2EC3/0/add1;S/SYMEC2EC3/Initialise
  /// ST_SYMEC2EC3/0/add2;S/SYMEC2EC3/Fu2LowStop
  /// ST_SYMEC2EC3/10/add0;S/SYMEC2EC3/P2
  /// ST_SYMEC2EC3/10/add1;S/SYMEC2EC3/P3
  /// ST_SYMEC2EC3/10/add2;S/SYMEC2EC3/AKRZ
  /// ST_SYMEC2EC3/10/add3;S/SYMEC2EC3/AKRP
  /// ST_SYMEC2EC3/10/add4;S/SYMEC2EC3/AKPZ
  /// ST_SYMEC2EC3/10/add5;S/SYMEC2EC3/AKPP
  /// ST_SYMEC2EC3/10/add6;S/SYMEC2EC3/P2U
  /// ST_SYMEC2EC3/10/add7;S/SYMEC2EC3/P3U
  /// ST_SYMEC2EC3/10/add8;S/SYMEC2EC3/P2Z
  /// ST_SYMEC2EC3/10/add9;S/SYMEC2EC3/P3Z
  /// ST_SYMEC2EC3/10/add10;S/SYMEC2EC3/PKRZ
  /// ST_SYMEC2EC3/10/add11;S/SYMEC2EC3/PKRP
  /// ST_SYMEC2EC3/10/add12;S/SYMEC2EC3/P2KPZ
  /// ST_SYMEC2EC3/10/add13;S/SYMEC2EC3/P2KPP
  /// ST_SYMEC2EC3/10/add14;S/SYMEC2EC3/P3KPZ
  /// ST_SYMEC2EC3/10/add15;S/SYMEC2EC3/P3KPP
  /// ST_SYMEC2EC3/10/add16;S/SYMEC2EC3/F2U
  /// ST_SYMEC2EC3/10/add17;S/SYMEC2EC3/F3U
  /// ST_SYMEC2EC3/10/add18;S/SYMEC2EC3/F2
  /// ST_SYMEC2EC3/10/add19;S/SYMEC2EC3/F3
  /// ST_SYMEC2EC3/10/add20;S/SYMEC2EC3/FUS
  /// ST_SYMEC2EC3/10/add21;S/SYMEC2EC3/CONF/Pu2
  /// ST_SYMEC2EC3/10/add22;S/SYMEC2EC3/CONF/Pu3
  /// ST_SYMEC2EC3/10/add23;S/SYMEC2EC3/CONF/A2u
  /// ST_SYMEC2EC3/10/add24;S/SYMEC2EC3/CONF/Ao
  /// ST_SYMEC2EC3/10/add25;S/SYMEC2EC3/CONF/Az
  /// ST_SYMEC2EC3/10/add26;S/SYMEC2EC3/CONF/cycle
  /// ST_SYMEC2EC3/10/add27;S/SYMEC2EC3/CONF/F2n
  /// ST_SYMEC2EC3/10/add28;S/SYMEC2EC3/CONF/F3n
  /// ST_SYMEC2EC3/10/add29;S/SYMEC2EC3/CONF/P2kpzn
  /// ST_SYMEC2EC3/10/add30;S/SYMEC2EC3/CONF/P2kppn
  /// ST_SYMEC2EC3/10/add31;S/SYMEC2EC3/CONF/P3kpzn
  /// ST_SYMEC2EC3/10/add32;S/SYMEC2EC3/CONF/P3kppn
  /// ST_SYMEC2EC3/10/add33;S/SYMEC2EC3/CONF/P2zn
  /// ST_SYMEC2EC3/10/add34;S/SYMEC2EC3/CONF/P3zn
  /// ST_SYMEC2EC3/10/add35;S/SYMEC2EC3/CONF/P3pn
  /// ST_SYMEC2EC3/10/add36;S/SYMEC2EC3/CONF/P3Stat
  /// ST_SYMEC2EC3/10/add37;S/SYMEC2EC3/CONF/Pu
  /// ST_SYMEC2EC3/10/add38;S/SYMEC2EC3/CONF/R2p
  /// ST_SYMEC2EC3/10/add39;S/SYMEC2EC3/CONF/R2z
  /// ST_SYMEC2EC3/10/add40;S/SYMEC2EC3/CONF/R3p
  /// ST_SYMEC2EC3/10/add41;S/SYMEC2EC3/CONF/R3z
  /// ST_SYMEC2EC3/10/add42;S/SYMEC2EC3/CONF/R2o
  /// ST_SYMEC2EC3/10/add43;S/SYMEC2EC3/CONF/R3o
  /// ST_SYMEC2EC3/10/add44;S/SYMEC2EC3/CONF/R3u
  /// ST_SYMEC2EC3/10/add45;S/SYMEC2EC3/CONF/Tdz
  /// ST_SYMEC2EC3/10/add46;S/SYMEC2EC3/CONF/Tdu
  /// ST_SYMEC2EC3/10/add47;S/SYMEC2EC3/CONF/F2_other_direction
  /// ST_SYMEC2EC3/10/add48;S/SYMEC2EC3/CONF/F3_other_direction
  /// ST_SYMEC2EC3/10/add49;S/SYMEC2EC3/EC2_SUMA_F_WODA
  /// ST_SYMEC2EC3/10/add50;S/SYMEC2EC3/EC3_SUMA_F_WODA
  /// ST_SYMEC2EC3/10/add51;S/SYMEC2EC3/AKPZO
  /// ST_SYMEC2EC3/10/add52;S/SYMEC2EC3/AKPPO
  /// ST_SYMEC2EC3/10/add53;S/SYMEC2EC3/CONF/RKPC
  /// ST_SYMEC2EC3/10/add54;S/SYMEC2EC3/CONF/RKRC
  /// ST_SYMEC2EC3/10/add55;S/SYMEC2EC3/CONF/PUWYS
  /// ST_SYMEC2EC3/10/add56;S/SYMEC2EC3/Rkpp_act
  /// ST_SYMEC2EC3/10/add57;S/SYMEC2EC3/Rkpz_act
  /// ST_SYMEC2EC3/10/add58;S/SYMEC2EC3/Rkrp_act
  /// ST_SYMEC2EC3/10/add59;S/SYMEC2EC3/Rkrz_act
  /// ST_SYMEC2EC3/10/add60;S/SYMEC2EC3/Ru2
  /// ST_SYMEC2EC3/10/add61;S/SYMEC2EC3/Pu2_act
  /// ST_SYMEC2EC3/10/add62;S/SYMEC2EC3/PKPStat
  /// 
  ///
  ///  Zalecane – przyk³adowe nastawy
  ///  Poni¿ej zamieszczono dwie propozycje nastaw (parametry dobierano w oparciu o aktualne warunki w dniu 14.11.2005)
  /// 
  ///  R2o = 2.5
  ///  R2p = 0.1
  ///  R2z = 0.3
  ///  R3o = 0.5
  ///  R3p = 0.1
  ///  R3z = 0.2
  ///  P2 = 800
  ///  P3 = 780
  /// 
  ///  lub
  ///  R2o = 1.6
  ///  R2p = 0.1
  ///  R2z = 0.3
  ///  R3o = 0.4
  ///  R3p = 0.1
  ///  R3z = 0.2
  ///  P2 = 850
  ///  P3 = 820
  /// </summary>
  internal class Symulator
  {
    #region PRIVATE
    internal double[] values = new double[ 63 ]; //przechowywanie wartosci danych analogowych dla symulatora  
    internal bool[] valuesCMD = new bool[ 10 ];     //przechowywanie rozkazow (resetowanie symulatora - inicjowanie parametrow itp)
    private class Integrator
    {
      private double sum;
      private readonly double mTd;
      internal void count( double input )
      {
        sum = sum + input * mTd;
      }
      internal double OVal { get { return sum; } }
      internal Integrator( double Td, double startVal )
      {
        sum = startVal;
        mTd = Td;
      }
    }
    class Const
    {
      internal const double Ao = 10000.0;
      internal const double Az = 0.0;
      internal const double Azmin = ( Ao - Az ) * 0.5 / 100.0;
      internal const uint cycle = 100;  //in ms
      internal const double F2n = 250.0;         // EC2 znamionowy przep³yw
      internal const double F3n = 1210.0;        // EC3 znamionowy przep³yw
      internal const double P2kpzn = 820.0;
      internal const double P2kppn = 140.0;
      internal const double P3kpzn = 1020.0;
      internal const double P3kppn = 240.0;
      internal const double P2zn = 1120;//910.0;
      internal const double P3zn = 1000.0;
      internal const double P3Stat = 100.0;
      internal const double Pu = 220.0;
      // opory hydrauliczne 
      internal const double R2p = 0.1; //(Pu - P2kppn)/F2n;
      internal const double R2z = 0.1; //R2p;
      internal const double R3p = 0.1; //(Pu + P3Stat - P3kppn)/F3n;
      internal const double R3z = 0.1; //R3p;
      internal const double R2o = ( P2kpzn - P2kppn ) / F2n;//*10 - by bylo wiêksze obci¹zenie
      internal const double R3o = ( P3kpzn - P3kppn ) / F3n;
      internal const double R3u = 100.0 / 3000.0;
      internal const double Tdz = F2n / ( P2zn - Pu ) / ( 1000 * 20 / cycle );
      internal const double Tdu = Tdz / 5.0;
      internal const double F2_other = 1000.0;
      internal const double F3_other = 5000.0;
      internal const double Rkpc = 80.0;
      internal const double Rkrc = 80.0;
      internal const double Puwys = 300;
      internal const double PKPstat = 0;
    }
    /// <summary>
    /// for values from const class initiation
    /// </summary>
    private void InitialisationMasterValues()
    {
      values[ (int)signalsIdx.Ao ] = Const.Ao;
      values[ (int)signalsIdx.Az ] = Const.Az;
      Azmin = Const.Azmin;
      values[ (int)signalsIdx.cycle ] = Const.cycle;
      values[ (int)signalsIdx.F2n ] = Const.F2n;
      values[ (int)signalsIdx.F3n ] = Const.F3n;
      values[ (int)signalsIdx.P2kpzn ] = Const.P2kpzn;
      values[ (int)signalsIdx.P2kppn ] = Const.P2kppn;
      values[ (int)signalsIdx.P3kpzn ] = Const.P3kpzn;
      values[ (int)signalsIdx.P3kppn ] = Const.P3kppn;
      values[ (int)signalsIdx.P2zn ] = Const.P2zn;
      values[ (int)signalsIdx.P3zn ] = Const.P3zn;
      values[ (int)signalsIdx.P3Stat ] = Const.P3Stat;
      values[ (int)signalsIdx.Pu ] = Const.Pu;
      values[ (int)signalsIdx.R2p ] = Const.R2p;
      values[ (int)signalsIdx.R2z ] = Const.R2z;
      values[ (int)signalsIdx.R3p ] = Const.R3p;
      values[ (int)signalsIdx.R3z ] = Const.R3z;
      values[ (int)signalsIdx.R2o ] = Const.R2o;
      values[ (int)signalsIdx.R3o ] = Const.R3o;
      values[ (int)signalsIdx.R3u ] = Const.R3u;
      values[ (int)signalsIdx.Tdz ] = Const.Tdz;
      values[ (int)signalsIdx.Tdu ] = Const.Tdu;
      values[ (int)signalsIdx.Fus ] = 5.0; //50.0;
      values[ (int)signalsIdx.F2_other_direction ] = Const.F2_other;
      values[ (int)signalsIdx.F3_other_direction ] = Const.F3_other;
      values[ (int)signalsIdx.Rkpc ] = Const.Rkpc;
      values[ (int)signalsIdx.Rkrc ] = Const.Rkrc;
      values[ (int)signalsIdx.Puwys ] = Const.Puwys;
      values[ (int)signalsIdx.PKPstat ] = Const.PKPstat;
    }
    private void ResetAllValues()
    {
      for ( int idx = 0; idx < values.Length; idx++ )
      {
        values[ idx ] = 0;
      }
      for ( int idx = 0; idx < valuesCMD.Length; idx++ )
      {
        valuesCMD[ idx ] = false;
      }
    }
    private void ResetSoft()
    {
      //      InitialisationMasterValues();
      F2i = new Integrator( values[ (int)signalsIdx.Tdz ], values[ (int)signalsIdx.F2n ] );
      F3i = new Integrator( values[ (int)signalsIdx.Tdz ], values[ (int)signalsIdx.F3n ] );
      Fui = new Integrator( values[ (int)signalsIdx.Tdu ], 0 );
      values[ (int)signalsIdx.P2 ] = values[ (int)signalsIdx.P2zn ] - values[ (int)signalsIdx.Pu ];
      values[ (int)signalsIdx.P3 ] = values[ (int)signalsIdx.P3zn ] - values[ (int)signalsIdx.Pu ];
      values[ (int)signalsIdx.Pu2 ] = (int)values[ (int)signalsIdx.Pu ];
      values[ (int)signalsIdx.Pu3 ] = (int)values[ (int)signalsIdx.Pu ];
      values[ (int)signalsIdx.A2u ] = values[ (int)signalsIdx.Ao ];
      values[ (int)signalsIdx.Akrz ] = values[ (int)signalsIdx.Ao ];
      values[ (int)signalsIdx.Akrp ] = values[ (int)signalsIdx.Ao ];
      values[ (int)signalsIdx.Akpz ] = values[ (int)signalsIdx.Az ];
      values[ (int)signalsIdx.Akpp ] = values[ (int)signalsIdx.Az ];
      values[ (int)signalsIdx.Akpzo ] = values[ (int)signalsIdx.Az ];
      values[ (int)signalsIdx.Akppo ] = values[ (int)signalsIdx.Az ];
    }
    private void Initialisation()
    {
      ResetAllValues();
      InitialisationMasterValues();
      ResetSoft();
    }

    private double Azmin = Const.Azmin;
    private Integrator F2i;
    private Integrator F3i;
    private Integrator Fui;
    /// <summary>
    /// wyznacza opor zaworu 
    /// </summary>
    /// <param name="alfa">kat </param>
    /// <param name="coefficient">wspolczynnik oporu</param>
    /// <param name="main">true - if it is main valve</param>
    /// <returns></returns>
    private double valve( double alfa, double coefficient, bool main )
    {
      if ( main )
      {
        return coefficient * ( values[ (int)signalsIdx.Ao ] - alfa ) / Math.Pow( Math.Max( alfa, Azmin ), 2 );
        //return ((coefficient/Math.Max(alfa, Azmin))-(coefficient/values[(int)signalsIdx.Ao]));
      }
      else
      {
        // jezeli amy doczynienia z odciaznikiem to jako opor zwracamy sume 10 procentowego 
        //otwarcia zaworu glownego + wartosc wynikajaca z aktualnego otwarcia)
        return valve( alfa, coefficient, true ) + valve( 100, coefficient, true );
      }
    }
    /// <summary>
    /// wyznacza opor zaworu i jego odciazniaka
    /// </summary>
    /// <param name="alfa">kat zaworu glownego</param>
    /// <param name="alfa2">kat odciaznika</param>
    /// <param name="coefficient">wspolczynnik oporu</param>
    /// <returns></returns>
    private double valve2( double alfa, double alfa2, double coefficient )
    {
      double R1 = valve( alfa, coefficient, true );
      double R2 = valve( alfa2, coefficient, false );
      return ( R1 * R2 ) / ( R1 + R2 );
    }

    private void CountP( double Rkrz, double Rkrp, double F2p, double F3p, double dP23u, double P3u )
    {
      values[ (int)signalsIdx.P2u ] = dP23u + P3u;
      values[ (int)signalsIdx.P3u ] = P3u - values[ (int)signalsIdx.P3Stat ];
      values[ (int)signalsIdx.P2z ] = values[ (int)signalsIdx.P2 ] + values[ (int)signalsIdx.P2u ];
      values[ (int)signalsIdx.P3z ] = values[ (int)signalsIdx.P3 ] + P3u;
      values[ (int)signalsIdx.Pkrp ] = values[ (int)signalsIdx.P2u ] + F2p * Rkrp;
      values[ (int)signalsIdx.P2kpp ] = values[ (int)signalsIdx.Pkrp ] + F2p * values[ (int)signalsIdx.R2p ] + values[ (int)signalsIdx.PKPstat ];
      values[ (int)signalsIdx.Pkrz ] = values[ (int)signalsIdx.P2z ] - F2i.OVal * Rkrz;
      values[ (int)signalsIdx.P2kpz ] = values[ (int)signalsIdx.Pkrz ] - F2i.OVal * values[ (int)signalsIdx.R2z ] + values[ (int)signalsIdx.PKPstat ];
      values[ (int)signalsIdx.P3kpz ] = values[ (int)signalsIdx.P3z ] - F3i.OVal * values[ (int)signalsIdx.R3z ] + values[ (int)signalsIdx.PKPstat ];
      values[ (int)signalsIdx.P3kpp ] = P3u + F3i.OVal * values[ (int)signalsIdx.R3p ] + values[ (int)signalsIdx.PKPstat ];
      values[ (int)signalsIdx.F2u ] = System.Math.Max( Fui.OVal + values[ (int)signalsIdx.Fus ], 0 );
      values[ (int)signalsIdx.F3u ] = System.Math.Max( -Fui.OVal + values[ (int)signalsIdx.Fus ], 0 );
      values[ (int)signalsIdx.F2 ] = F2i.OVal;
      values[ (int)signalsIdx.F3 ] = F3i.OVal;
      values[ (int)signalsIdx.F2_all ] = values[ (int)signalsIdx.F2 ] + values[ (int)signalsIdx.F2_other_direction ];
      values[ (int)signalsIdx.F3_all ] = values[ (int)signalsIdx.F3 ] + values[ (int)signalsIdx.F3_other_direction ];
    }
    private double R2u_calculate( double P, ref  double Pu, double Pwys )
    {
      if ( P <= Pu || !valuesCMD[ (int)commandsIdx.Fu2LowStop ] )  // jest odkrecony dolny przelew lub cisnienie opdowiednio nieksie
      {
        Pu = values[ (int)signalsIdx.Pu ]; //sprawdzic czy Pu
        return valve( values[ (int)signalsIdx.A2u ], values[ (int)signalsIdx.Rkrc ], true );
      }
      else
      {
        if ( P <= Pwys )
        {
          Pu = values[ (int)signalsIdx.Pu2 ];//sprawdzic czy Pu
          return valve( values[ (int)signalsIdx.A2u ], values[ (int)signalsIdx.Rkrc ], true );
          ;
        }
        else
        {
          Pu = 0;
          return valve( values[ (int)signalsIdx.A2u ], values[ (int)signalsIdx.Rkrc ], true );
        }
      }
    }
    private void CountSymulator()
    {
      while ( true )
      {
        try
        {
          //sprawdzanie czy zainstnialy powody do resetu lub inicjalizacji
          if ( valuesCMD[ (int)commandsIdx.ResetAll ] )
          {
            Initialisation();
            valuesCMD[ (int)commandsIdx.ResetAll ] = false;
          }
          if ( valuesCMD[ (int)commandsIdx.ResetToCurrent ] )
          {
            ResetSoft();
            valuesCMD[ (int)commandsIdx.ResetToCurrent ] = false;
          }

          values[ (int)signalsIdx.Rkpz_act ] = valve2( values[ (int)signalsIdx.Akpz ], values[ (int)signalsIdx.Akpzo ], values[ (int)signalsIdx.Rkpc ] );
          values[ (int)signalsIdx.Rkpp_act ] = valve2( values[ (int)signalsIdx.Akpp ], values[ (int)signalsIdx.Akppo ], values[ (int)signalsIdx.Rkpc ] );
          values[ (int)signalsIdx.Rkrz_act ] = valve( values[ (int)signalsIdx.Akrz ], values[ (int)signalsIdx.Rkrc ], true );
          values[ (int)signalsIdx.Rkrp_act ] = valve( values[ (int)signalsIdx.Akrp ], values[ (int)signalsIdx.Rkrc ], true );
          //          double R2u    = valve(values[(int)signalsIdx.A2u  ],values[(int)signalsIdx.Rkrc ],true);
          double Pu2_temp = values[ (int)signalsIdx.Pu2 ];
          values[ (int)signalsIdx.Ru2 ] = R2u_calculate( values[ (int)signalsIdx.P2u ], ref Pu2_temp, values[ (int)signalsIdx.Puwys ] );
          values[ (int)signalsIdx.Pu2_act ] = Pu2_temp;
          double mian = values[ (int)signalsIdx.Rkpp_act ] + values[ (int)signalsIdx.Rkpz_act ] + values[ (int)signalsIdx.R2o ] + values[ (int)signalsIdx.R3o ];
          double F2o = ( ( F2i.OVal * ( values[ (int)signalsIdx.R3o ] + values[ (int)signalsIdx.Rkpp_act ] + values[ (int)signalsIdx.Rkpz_act ] ) + F3i.OVal * values[ (int)signalsIdx.R3o ] - Fui.OVal * values[ (int)signalsIdx.Rkpp_act ] ) / mian );
          double F3o = ( ( F3i.OVal * ( values[ (int)signalsIdx.R2o ] + values[ (int)signalsIdx.Rkpp_act ] + values[ (int)signalsIdx.Rkpz_act ] ) + F2i.OVal * values[ (int)signalsIdx.R2o ] + Fui.OVal * values[ (int)signalsIdx.Rkpp_act ] ) / mian );
          double Fkpp = ( F2i.OVal * values[ (int)signalsIdx.R2o ] - F3i.OVal * values[ (int)signalsIdx.R3o ] - Fui.OVal * ( values[ (int)signalsIdx.R2o ] + values[ (int)signalsIdx.R3o ] + values[ (int)signalsIdx.Rkpz_act ] ) ) / mian;
          double F2p = F2o + Fkpp;
          double F3p = F3o - Fkpp;
          double P2p = F2p * ( values[ (int)signalsIdx.Rkrp_act ] + values[ (int)signalsIdx.R2p ] );
          double P3p = F3p * values[ (int)signalsIdx.R3p ];
          double dPkpp = Fkpp * values[ (int)signalsIdx.Rkpp_act ];
          double dP23u = -P2p + P3p - dPkpp;
          double P3u = Fui.OVal * values[ (int)signalsIdx.R3u ] + values[ (int)signalsIdx.Pu3 ] + values[ (int)signalsIdx.P3Stat ];
          CountP( values[ (int)signalsIdx.Rkrz_act ], values[ (int)signalsIdx.Rkrp_act ], F2p, F3p, dP23u, P3u );
          double dP2 = F2o * values[ (int)signalsIdx.R2o ] + P2p + F2i.OVal * ( values[ (int)signalsIdx.Rkrz_act ] + values[ (int)signalsIdx.R2z ] ) - values[ (int)signalsIdx.P2 ];
          F2i.count( -dP2 );
          double dP3 = F3o * values[ (int)signalsIdx.R3o ] + P3p + F3i.OVal * values[ (int)signalsIdx.R3z ] - values[ (int)signalsIdx.P3 ];
          F3i.count( -dP3 );
          double dPu = dP23u + Fui.OVal * values[ (int)signalsIdx.Ru2 ] + P3u - Pu2_temp;
          Fui.count( -dPu );
          System.Threading.Thread.Sleep( TimeSpan.FromMilliseconds( values[ (int)signalsIdx.cycle ] ) );
        }
        catch ( Exception )
        {
          EventLogMonitor.WriteToEventLogInfo( "Ec2Ec3Symulator has done wrong operation and it was restarted",
            (int)CAS.Lib.RTLib.Processes.Error.CommServer_EC2EC3_symulator );
          Initialisation();
        }
      }
    }
    internal enum signalsIdx: int
    {
      P2 = 0,
      P3 = 1,
      Akrz = 2,
      Akrp = 3,
      Akpz = 4,
      Akpp = 5,
      P2u = 6,
      P3u = 7,
      P2z = 8,
      P3z = 9,
      Pkrz = 10,
      Pkrp = 11,
      P2kpz = 12,
      P2kpp = 13,
      P3kpz = 14,
      P3kpp = 15,
      F2u = 16,
      F3u = 17,
      F2 = 18,
      F3 = 19,
      Fus = 20,
      Pu2 = 21,
      Pu3 = 22,
      A2u = 23, // klapa uzupelnien w E2?
      Ao = 24,
      Az = 25,
      cycle = 26,
      F2n = 27,
      F3n = 28,
      P2kpzn = 29,
      P2kppn = 30,
      P3kpzn = 31,
      P3kppn = 32,
      P2zn = 33,
      P3zn = 34,
      P3pn = 35,
      P3Stat = 36,
      Pu = 37,
      R2p = 38,
      R2z = 39,
      R3p = 40,
      R3z = 41,
      R2o = 42,
      R3o = 43,
      R3u = 44,
      Tdz = 45,
      Tdu = 46,
      F2_other_direction = 47, // przeplyw  konfigurowalny dla pozostalych kierunkow w EC2 - nie ma wplywu na prace symulatora
      F3_other_direction = 48, // przeplyw  konfigurowalny dla pozostalych kierunkow w EC3 - nie ma wplywu na prace symulatora
      F2_all = 49, // calkowity przeplyw    EC2 - nie ma wplywu na prace symulatora
      F3_all = 50,  // calkowity przeplyw    EC3 - nie ma wplywu na prace symulatora
      Akpzo = 51, //polorzenie odciaznika w komorze podzialowej
      Akppo = 52,  //porzenie odciaznika w komorze podzialowej
      Rkpc = 53, //wspolczynnik oporu zasuw dla komory podzialowej
      Rkrc = 54, //wspolczynnik oporu zasuw dla komory regulacyjnej
      Puwys = 55, //cisnienie wysokiego przelewu
      Rkpp_act = 56, //opor zamienny w komorze podzialowej na powrocie  
      Rkpz_act = 57, //opor zamienny w komorze podzialowej na powrocie  
      Rkrp_act = 58, //opor zamienny w komorze podzialowej na powrocie  
      Rkrz_act = 59, //opor zamienny w komorze podzialowej na powrocie  
      Ru2 = 60, //opor stawiany przez uzupelnienia na ec2
      Pu2_act = 61,  //aktualne cisnienie uzupelnien na powrocie ec2
      PKPstat = 62    //cisnienie statyczne w komorze podzialowej
    }
    internal enum commandsIdx: int
    {
      ResetAll = 0,  //initialise reset of all values in the simulator
      ResetToCurrent = 1,    //
      Fu2LowStop = 2   // zakreca dolny przelew na EC2
    }
    #endregion
    /// <summary>
    /// Constructor fo Symulator
    /// </summary>
    internal Symulator()
    {
      Initialisation();
      Manager.StartProcess( new System.Threading.ThreadStart( CountSymulator ) );
      //Processes.EventLogMonitor.WriteToEventLogInfo( "EC2 EC3 Simulator crated",
      //  (int)Processes.Error.CommServer_EC2EC3_symulator );
    }
  }
}
