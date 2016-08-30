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
//    <Author> - <date>:
//    <description>
//
//  Copyright (C)2006, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.com.pl
//  http:\\www.cas.eu
//</summary>

using System;
using CAS.Lib.RTLib.Processes;

namespace CAS.Lib.CommonBus.ApplicationLayer.EC2_3SYM2
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
  ///  93;1000;10000;1000;10000;0;10;51
  ///  93;1000;10000;1000;10000;0;0;5
  ///  Mapowanie nazw
  /// 
  ///  ST_SYMEC2EC3/0/add1;S/SYMEC2EC3/Initialise
  ///  ST_SYMEC2EC3/10/add0;S/SYMEC2EC3/P2
  ///  ST_SYMEC2EC3/10/add1;S/SYMEC2EC3/P3
  ///  ST_SYMEC2EC3/10/add2;S/SYMEC2EC3/AKRZ
  ///  ST_SYMEC2EC3/10/add3;S/SYMEC2EC3/AKRP
  ///  ST_SYMEC2EC3/10/add4;S/SYMEC2EC3/AKPZ
  ///  ST_SYMEC2EC3/10/add5;S/SYMEC2EC3/AKPP
  ///  ST_SYMEC2EC3/10/add6;S/SYMEC2EC3/P2U
  ///  ST_SYMEC2EC3/10/add7;S/SYMEC2EC3/P3U
  ///  ST_SYMEC2EC3/10/add8;S/SYMEC2EC3/P2Z
  ///  ST_SYMEC2EC3/10/add9;S/SYMEC2EC3/P3Z
  ///  ST_SYMEC2EC3/10/add10;S/SYMEC2EC3/PKRZ
  ///  ST_SYMEC2EC3/10/add11;S/SYMEC2EC3/PKRP
  ///  ST_SYMEC2EC3/10/add12;S/SYMEC2EC3/P2KPZ
  ///  ST_SYMEC2EC3/10/add13;S/SYMEC2EC3/P2KPP
  ///  ST_SYMEC2EC3/10/add14;S/SYMEC2EC3/P3KPZ
  ///  ST_SYMEC2EC3/10/add15;S/SYMEC2EC3/P3KPP
  ///  ST_SYMEC2EC3/10/add16;S/SYMEC2EC3/F2U
  ///  ST_SYMEC2EC3/10/add17;S/SYMEC2EC3/F3U
  ///  ST_SYMEC2EC3/10/add18;S/SYMEC2EC3/F2
  ///  ST_SYMEC2EC3/10/add19;S/SYMEC2EC3/F3
  ///  ST_SYMEC2EC3/10/add20;S/SYMEC2EC3/FUS
  ///  ST_SYMEC2EC3/10/add21;S/SYMEC2EC3/CONF/Pu2
  ///  ST_SYMEC2EC3/10/add22;S/SYMEC2EC3/CONF/Pu3
  ///  ST_SYMEC2EC3/10/add23;S/SYMEC2EC3/CONF/A2u
  ///  ST_SYMEC2EC3/10/add24;S/SYMEC2EC3/CONF/Ao
  ///  ST_SYMEC2EC3/10/add25;S/SYMEC2EC3/CONF/Az
  ///  ST_SYMEC2EC3/10/add26;S/SYMEC2EC3/CONF/cycle
  ///  ST_SYMEC2EC3/10/add27;S/SYMEC2EC3/CONF/F2n
  ///  ST_SYMEC2EC3/10/add28;S/SYMEC2EC3/CONF/F3n
  ///  ST_SYMEC2EC3/10/add29;S/SYMEC2EC3/CONF/P2kpzn
  ///  ST_SYMEC2EC3/10/add30;S/SYMEC2EC3/CONF/P2kppn
  ///  ST_SYMEC2EC3/10/add31;S/SYMEC2EC3/CONF/P3kpzn
  ///  ST_SYMEC2EC3/10/add32;S/SYMEC2EC3/CONF/P3kppn
  ///  ST_SYMEC2EC3/10/add33;S/SYMEC2EC3/CONF/P2zn
  ///  ST_SYMEC2EC3/10/add34;S/SYMEC2EC3/CONF/P3zn
  ///  ST_SYMEC2EC3/10/add35;S/SYMEC2EC3/CONF/P3pn
  ///  ST_SYMEC2EC3/10/add36;S/SYMEC2EC3/CONF/P3Stat
  ///  ST_SYMEC2EC3/10/add37;S/SYMEC2EC3/CONF/Pu
  ///  ST_SYMEC2EC3/10/add38;S/SYMEC2EC3/CONF/R2p
  ///  ST_SYMEC2EC3/10/add39;S/SYMEC2EC3/CONF/R2z
  ///  ST_SYMEC2EC3/10/add40;S/SYMEC2EC3/CONF/R3p
  ///  ST_SYMEC2EC3/10/add41;S/SYMEC2EC3/CONF/R3z
  ///  ST_SYMEC2EC3/10/add42;S/SYMEC2EC3/CONF/R2o
  ///  ST_SYMEC2EC3/10/add43;S/SYMEC2EC3/CONF/R3o
  ///  ST_SYMEC2EC3/10/add44;S/SYMEC2EC3/CONF/R3u
  ///  ST_SYMEC2EC3/10/add45;S/SYMEC2EC3/CONF/Tdz
  ///  ST_SYMEC2EC3/10/add46;S/SYMEC2EC3/CONF/Tdu
  ///  ST_SYMEC2EC3/10/add47;S/SYMEC2EC3/CONF/F2_other_direction
  ///  ST_SYMEC2EC3/10/add48;S/SYMEC2EC3/CONF/F3_other_direction
  ///  ST_SYMEC2EC3/10/add49;S/SYMEC2EC3/EC2_SUMA_F_WODA
  ///  ST_SYMEC2EC3/10/add50;S/SYMEC2EC3/EC3_SUMA_F_WODA
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
    private class Integrator
    {
      private double sum;
      private readonly double mTd;
      internal void count( double input )
      {
        sum = sum + input / mTd;
      }
      internal double OVal { get { return sum; } }
      internal Integrator( double startVal ) //(double Tp, double startVal)
      {
        sum = startVal;
        //stala czasowa calkowania
        mTd = 1 / Tp;
      }
    }

    //wspolczynnik przeliczania cisnien 1MPa=1000kPa
    private const int wsp = 1000;
    //okres probkowania modelu w [min]
    private const double Tp = 0.02;

    //parametry
    //cisnienia zasilania
    private const double P3zd = wsp * 1.15;
    private const double P2zd = wsp * 1.20;
    //cisnienie statyczne
    private const double Pstat = wsp * 0.09;
    //opory rurociagow
    private const double R3 = wsp * ( 1.15 - 0.9 ) / 4000;
    private const double R2 = wsp * ( ( 1.20 - 0.09 ) - 1.03 ) / 1500;
    //opory obciazen
    private const double R3o = ( wsp * 0.9 - R3 * 3970 - wsp * 0.25 ) / 4000;
    private const double R2o = ( wsp * 1.03 - ( wsp * 0.23 + R2 * 1485 - wsp * 0.09 ) ) / 1500;
    //masy wody w sieciach
    private readonly double M3o = wsp * 5 * Math.Pow( 10, -4 );
    private readonly double M2o = wsp * 2 * Math.Pow( 10, -4 );
    //wzmocnienie regulatorow uzupelnien
    private readonly static double Kr = 3 * Math.Pow( 10, 4 ) / wsp;
    //wspolczynnik powierzchni zbiornikow uzupelnien
    private readonly double Mu = Math.Pow( 10, 4 ) / wsp;
    //cisnienia zadane na powrocie
    private readonly double P3pd = 30 / Kr + 0.25 * wsp;
    private readonly double P2pd = 15 / Kr + 0.23 * wsp;
    //parametry zasuw podzialowych
    private readonly double RZ = wsp * ( Math.Pow( 10, -6 ) );
    private readonly double MZ = wsp * Math.Pow( 10, -2 );
    //minimalna wartosc kata zamkniecia zasuw
    private const double Amin = 10;
    //wartosc kata pelnego otwarcia zasuw
    private const double Amax = 10000;
    //warunki poczatkowe integratorow
    private const double F3n = 4000;
    private const double F2n = 1500;
    private const double P3un = wsp * 0.25;
    private const double P2un = wsp * 0.23;
    private const double Fzn = 0;
    private const double Fpn = 0;
    //straty w sieciach
    private const double F3ud = 30;
    private const double F2ud = 15;

    //integratory
    private Integrator F2i;
    private Integrator F3i;
    private Integrator P3ui;
    private Integrator P2ui;
    private Integrator Fzi;
    private Integrator Fpi;

    #region stare
    /*
    //stare stale
	private const uint cycle    = 2 * 60 * 10;  //in ms
    //private const double F2n    = 500;         // EC2 znamionowy przep³yw
    //private const double F3n    = 3000;        // EC3 znamionowy przep³yw
    private const double P2kpzn = 820;
    private const double P2kppn = 140;
    private const double P3kpzn = 1020;
    private const double P3kppn = 240;
    private const double P2zn   = 910;
    private const double P3zn   = 1140;
    private const double P3Stat = 100;
    private const double Pu     = 220;
    private const double R2p    = (Pu - P2kppn)/F2n;
    private const double R2z    = R2p;
    private const double R3p    = (Pu + P3Stat - P3kppn)/F3n;
    private const double R3z    = R3p;
    //private const double R2o    = ( P2kpzn - P2kppn ) / F2n;
    //private const double R3o    = ( P3kpzn - P3kppn ) / F3n;
    private const double Ru     = 100 / 3000;
    private const double Tdz    = F2n / (P2zn-Pu) /( 1000*60 / cycle);
    private const double Tdu    = Tdz;
//    private Integrator F2i;
//    private Integrator F3i;
    private Integrator Fui;
    //private Integrator Fzi;
    private Integrator Fp2;
    private double valve ( double alfa)
    {
      ;
      return 69*(10000-alfa)/Math.Pow( Math.Max(alfa, 50), 2);
    }
    private void CountP(double Rkrz, double Rkrp, double F2p, double F3p)
    {
      values[(int)signalsIdx.P2u   ] = Pu - Fui.OVal * Ru;
      values[(int)signalsIdx.P3u   ] = Pu + Fui.OVal * Ru + P3Stat;
      values[(int)signalsIdx.P2z   ] = values[(int)signalsIdx.P2   ] + values[(int)signalsIdx.P2u   ];
      values[(int)signalsIdx.P3z   ] = values[(int)signalsIdx.P3   ] + values[(int)signalsIdx.P3u   ];
      values[(int)signalsIdx.Pkrp  ] = values[(int)signalsIdx.P2u  ] + F2p * Rkrp;
      values[(int)signalsIdx.P2kpp ] = values[(int)signalsIdx.Pkrp ] + F2p * R2p;
      values[(int)signalsIdx.Pkrz  ] = values[(int)signalsIdx.P2z  ] - F2i.OVal * Rkrz;
      values[(int)signalsIdx.P2kpz ] = values[(int)signalsIdx.Pkrz ] - F2i.OVal * R2z;
      values[(int)signalsIdx.P3kpz ] = values[(int)signalsIdx.P3z  ] - F3i.OVal * R3z;
      values[(int)signalsIdx.P3kpp ] = values[(int)signalsIdx.P3u  ] + F3i.OVal * R3p;
      values[(int)signalsIdx.F2u   ] =  Fui.OVal + values[(int)signalsIdx.Fus ];
      values[(int)signalsIdx.F3u   ] = -Fui.OVal + values[(int)signalsIdx.Fus ];
      values[(int)signalsIdx.F2    ] = F2i.OVal;
      values[(int)signalsIdx.F3    ] = F3i.OVal;
    }*/
    #endregion
    private void CountSymulator()
    {
      //double Fzi0=0;
      //double Fpi0=0;
      //double P2kpz0=1030;
      while ( true )
      {
        //nowe obliczenia
        //EC3
        double P3z = P3zd + values[ (int)signalsIdx.P3 ];
        double F3 = F3i.OVal - Fzi.OVal;
        double F3p = F3i.OVal - F3ud - Fpi.OVal;
        double P3kpp = P3ui.OVal + R3 * F3p;
        double P3kpz = P3z - R3 * F3;
        double F3u = Kr * ( P3pd - P3ui.OVal );
        F3i.count( ( P3kpz - P3kpp - R3o * F3i.OVal ) / M3o );
        P3ui.count( ( F3p - F3 + F3u ) / Mu );
        values[ (int)signalsIdx.P3z ] = System.Convert.ToInt32( P3z );
        values[ (int)signalsIdx.F3 ] = System.Convert.ToInt32( F3 );
        values[ (int)signalsIdx.P3kpp ] = System.Convert.ToInt32( P3kpp );
        values[ (int)signalsIdx.P3kpz ] = System.Convert.ToInt32( P3kpz );
        values[ (int)signalsIdx.F3u ] = System.Convert.ToInt32( F3u );

        //EC2
        double P2z = P2zd + values[ (int)signalsIdx.P2 ];
        double F2 = F2i.OVal + Fzi.OVal;
        double F2p = F2i.OVal - F2ud - values[ (int)signalsIdx.Fus ] + Fpi.OVal;
        double Zkrp = values[ (int)signalsIdx.Akrp ];
        if ( values[ (int)signalsIdx.Akrp ] < Amin )
          Zkrp = Amin;
        double P2kpp = P2ui.OVal - Pstat + Amax * Amax * R2 * F2p / ( Zkrp * Zkrp );
        double Zkrz = values[ (int)signalsIdx.Akrz ];
        if ( values[ (int)signalsIdx.Akrz ] < Amin )
          Zkrz = Amin;
        double P2kpz = P2z - Pstat - Amax * Amax * R2 * F2 / ( Zkrz * Zkrz );
        double F2u = Kr * ( P2pd - P2ui.OVal );
        F2i.count( ( P2kpz - P2kpp - R2o * F2i.OVal ) / M2o );
        P2ui.count( ( F2p - F2 + F2u ) / Mu );
        values[ (int)signalsIdx.P2z ] = System.Convert.ToInt32( P2z );
        values[ (int)signalsIdx.F2 ] = System.Convert.ToInt32( F2 );
        values[ (int)signalsIdx.P2kpp ] = System.Convert.ToInt32( P2kpp );
        values[ (int)signalsIdx.P2kpz ] = System.Convert.ToInt32( P2kpz );
        values[ (int)signalsIdx.F2u ] = System.Convert.ToInt32( F2u );

        //komora podzialowa
        double Zkpz = values[ (int)signalsIdx.Akpz ];
        if ( values[ (int)signalsIdx.Akpz ] < Amin )
          Zkpz = Amin;
        Fzi.count( ( P2kpz - P3kpz - Amax * Amax * RZ * Fzi.OVal / ( Zkpz * Zkpz ) ) / MZ );
        double Zkpp = values[ (int)signalsIdx.Akpp ];
        if ( values[ (int)signalsIdx.Akpp ] < Amin )
          Zkpp = Amin;
        Fpi.count( ( P3kpp - P2kpp - Amax * Amax * RZ * Fpi.OVal / ( Zkpp * Zkpp ) ) / MZ );

        #region stare
        /*
		//stare obliczenia
        double Rkpz = valve(values[(int)signalsIdx.Akpz ]);
        double Rkpp = valve(values[(int)signalsIdx.Akpp ]);
        double Rkrz = valve(values[(int)signalsIdx.Akrz ]);
        double Rkrp = valve(values[(int)signalsIdx.Akrp ]);
        //double F2p = F2i.OVal - Fui.OVal;
        //double F3p = F3i.OVal + Fui.OVal;
        double mian = Rkpp + Rkpz + R2o +R3o;
        double F2o  = ((F2i.OVal*(R2o+Rkpp+Rkpz) + F3i.OVal*R3o - Fui.OVal*Rkpp ) / mian);
        double F3o  = ((F2i.OVal*R2o + F3i.OVal*(R3o+Rkpp+Rkpz) - Fui.OVal*Rkpp ) / mian);
        double P2p = F2p*(Rkrp+R2p);
        double P3p = F3p*R3p;
        double Fkpp  = - F2o + F2p;
        double Fkpp3 =   F3o - F3p;
        double Kkpz  = -F2i.OVal + F2o;
        double Fkpz3 =  F3i.OVal - F3o;
        CountP(Rkrz, Rkrp, F2p, F3p);
        double dP2   = F2o*R2o + P2p + F2i.OVal*(Rkrz+R2z) - values[(int)signalsIdx.P2];
        F2i.count(-dP2);
        double dP3 = F3o*R3o + P3p + F2i.OVal*R3z - values[(int)signalsIdx.P2 ];
        F3i.count(-dP3);
        double dPkp = values[(int)signalsIdx.P2kpp] - values[(int)signalsIdx.P3kpp];
        double dPu = 2*Fui.OVal*Ru - P2p + P3p - Fkpp*Rkpp + P3Stat;
//        Fui.count(-dPu);
		*/
        #endregion
        System.Threading.Thread.Sleep( TimeSpan.FromMinutes( Tp ) );

      }
    }
    internal double[] values = new double[ 25 ];
    internal enum signalsIdx: int
    {
      P2 = 0,
      P3,
      Akrz,
      Akrp,
      Akpz,
      Akpp,
      P2u,
      P3u,
      P2z,
      P3z,
      Pkrz,
      Pkrp,
      P2kpz,
      P2kpp,
      P3kpz,
      P3kpp,
      F2u,
      F3u,
      F2,
      F3,
      Fus
    }
    #endregion
    internal Symulator()
    {
      //integratory
      F2i = new Integrator( F2n );
      F3i = new Integrator( F3n );
      P3ui = new Integrator( P3un );
      P2ui = new Integrator( P2un );
      Fzi = new Integrator( Fzn );
      Fpi = new Integrator( Fpn );
      //wartosci poczatkowe
      values[ (int)signalsIdx.Akrz ] = 10000.0;
      values[ (int)signalsIdx.Akrp ] = 10000.0;
      values[ (int)signalsIdx.Akpz ] = 0.0;
      values[ (int)signalsIdx.Akpp ] = 0.0;
      //poczatkowe wartosci przyrostow cisnien zasilania
      values[ (int)signalsIdx.P2 ] = 100;
      values[ (int)signalsIdx.P3 ] = 100;
      //poczatkowa wartosc przyrostu strat w EC2
      values[ (int)signalsIdx.Fus ] = 5;
      Manager.StartProcess( new System.Threading.ThreadStart( CountSymulator ) );
    }
  }
}
