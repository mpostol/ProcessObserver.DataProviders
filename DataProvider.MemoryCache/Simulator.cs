//<summary>
//  Title   : Simulator
//  System  : Microsoft Visual C# .NET 2005
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//  2008: created
//
//  Copyright (C)2006, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using CAS.Lib.RTLib.Processes;
using System;
using System.Diagnostics;

namespace CAS.Lib.CommonBus.ApplicationLayer.DemoSimulator
{

  /// <summary>
  /// Class Simulator.
  /// </summary>
  internal class Simulator
  {
    //Zmienne
    internal double[] values ; //przechowywanie wartosci danych analogowych dla symulatora  
    internal bool[] valuesCMD;     //przechowywanie rozkazow (resetowanie symulatora - inicjowanie parametrow itp)


    /// <summary>
    /// Initializes a new instance of the <see cref="Simulator"/> class.
    /// </summary>
    internal Simulator()
    {
      try
      {
        Initialisation();
        Manager.StartProcess( new System.Threading.ThreadStart( CountSymulator ) );
      }
      catch ( Exception ex )
      {
        AssemblyTraceEvent.Tracer.TraceEvent(TraceEventType.Warning, 338, $"CAS.Lib.CommonBus.ApplicationLayer.DemoSimulator.Simulator: unable to start simulator because of exception: {ex.Message}" );
      }
    }
    internal bool TestCommunication( int station )
    {
      return ( valuesCMD[ (int)commandsIdx.droga_1 ] && station == 1 ) || ( valuesCMD[ (int)commandsIdx.droga_2 ] && station == 2 ) || ( station != 1 && station != 2 );
    }
    internal void TransmitterON( int station )
    {
      if ( station == 1 )
        valuesCMD[ (int)commandsIdx.droga_1_transmitterON ] = true;
      if ( station == 2 )
        valuesCMD[ (int)commandsIdx.droga_2_transmitterON ] = true;

    }
    internal void TransmitterOFF( int station )
    {
      if ( station == 1 )
        valuesCMD[ (int)commandsIdx.droga_1_transmitterON ] = false;
      if ( station == 2 )
        valuesCMD[ (int)commandsIdx.droga_2_transmitterON ] = false;
    }

    #region PRIVATE
    /// <summary>
    /// stale potrzebne przy inicjalizacji 
    /// </summary>
    class Const
    {
      internal const double pi = 3.1415;
      internal const double A_sin = 10;
      internal const double A_pil = 10;
      internal const double b = 7;
      internal const double w = 0.5;
      internal const uint cycle = 1000;  //in ms
      internal const double a1 = 2.3;
      internal const double a2 = 2.3;
      internal const double a3 = 2.3;
      internal const double a4 = 2.3;
      internal const double V1 = 60; //w procentach
      internal const double V2 = 60;
      internal const double k1 = 551;
      internal const double k2 = 668;
      internal const double A1 = 730;
      internal const double A2 = 730;
      internal const double A3 = 730;
      internal const double A4 = 730;
      internal const double g = 981;
      internal const double r1 = 0.133;
      internal const double r2 = 0.107;
      internal const int mn = 1;
      internal const double AlarmSetPoint = 0.05;
      internal const int TransmissionDelayInMs = 10;
    }
    /// <summary>
    /// for values from const class initiation
    /// </summary>
    private void InitialisationMasterValues()
    {
      values[(int)signalsIdx.A_sin] = Const.A_sin;
      values[(int)signalsIdx.A_pil] = Const.A_pil;
      values[(int)signalsIdx.t] = 0;
      values[(int)signalsIdx.w] = Const.w;
      values[(int)signalsIdx.b] = Const.b;
      values[(int)signalsIdx.cycle] = 1000;
      values[(int)signalsIdx.a1] = Const.a1;
      values[(int)signalsIdx.a2] = Const.a2;
      values[(int)signalsIdx.a3] = Const.a3;
      values[(int)signalsIdx.a4] = Const.a4;
      values[(int)signalsIdx.A1] = Const.A1;
      values[(int)signalsIdx.A2] = Const.A2;
      values[(int)signalsIdx.A3] = Const.A3;
      values[(int)signalsIdx.A4] = Const.A4;
      values[(int)signalsIdx.V1] = Const.V1;
      values[(int)signalsIdx.V2] = Const.V2;
      values[(int)signalsIdx.k1] = Const.k1;
      values[(int)signalsIdx.k2] = Const.k2;
      values[(int)signalsIdx.g] = Const.g;
      values[(int)signalsIdx.r1] = Const.r1;
      values[(int)signalsIdx.r2] = Const.r2;
      values[(int)signalsIdx.mn] = Const.mn;
      values[(int)signalsIdx.TransmissionDelayInMs] = Const.TransmissionDelayInMs;
      values[(int)signalsIdx.alarm_delta] = Const.AlarmSetPoint;
      valuesCMD[(int)commandsIdx.Alarm_switchON] = true;
      valuesCMD[(int)commandsIdx.droga_1] = true;
      valuesCMD[(int)commandsIdx.droga_2] = true;
      valuesCMD[(int)commandsIdx.droga_1_transmitterON] = false;
      valuesCMD[(int)commandsIdx.droga_2_transmitterON] = false;
    }

    private double Generator_sinus(double t)
    {
      //y=sin(wt*pi/4)
      return (values[(int)signalsIdx.A_sin] * Math.Sin(values[(int)signalsIdx.w]
          * t * Const.pi / 4));
    }
    private double Generator_pila(double t)
    {
      return (values[(int)signalsIdx.A_pil] * (t - (values[(int)signalsIdx.mn] - 1) * values[(int)signalsIdx.b])
     + (values[(int)signalsIdx.mn] - 1) * values[(int)signalsIdx.b]);
    }
    private void ResetAllValues()
    {
      for (int idx = 0; idx < values.Length; idx++)
      {
        values[idx] = 0;
      }
      for (int idx = 0; idx < valuesCMD.Length; idx++)
      {
        valuesCMD[idx] = false;
      }
    }
    //ResetSoft wyrzucilem
    private void Initialisation()
    {
      values = new double[Enum.GetValues(typeof(signalsIdx)).Length];
      valuesCMD = new bool[Enum.GetValues(typeof(commandsIdx)).Length];
      ResetAllValues();
      InitialisationMasterValues();
    }
    private void CountSymulator()
    {
      while (true)
      {
        try
        {
          #region resetowanie
          //sprawdzanie czy zainstnialy powody do resetu lub inicjalizacji
          if (valuesCMD[(int)commandsIdx.ResetAll])
          {
            Initialisation();
            valuesCMD[(int)commandsIdx.ResetAll] = false;
          }
          if (valuesCMD[(int)commandsIdx.ResetToCurrent])
          {
            InitialisationMasterValues();
            valuesCMD[(int)commandsIdx.ResetToCurrent] = false;
          }
          #endregion resetowanie
          #region rownania symulatora
          //  h3 = -(a3 * cycle) / (A3 * 1000) * Math.Sqrt(2 * g * h3) + ((1 - r2) * k2 * V2/100 * cycle)
          //  / (A3 * 1000) + poprz_h3;
          values[(int)signalsIdx.h3] = -(values[(int)signalsIdx.a3] * values[(int)signalsIdx.cycle])
              / (values[(int)signalsIdx.A3] * 1000) * Math.Sqrt(2 * values[(int)signalsIdx.g] *
              values[(int)signalsIdx.h3]) + ((1 - values[(int)signalsIdx.r2]) *
              values[(int)signalsIdx.k2] * values[(int)signalsIdx.V2] / 100 * values[(int)signalsIdx.cycle])
              / (values[(int)signalsIdx.A3] * 1000) + values[(int)signalsIdx.poprz_h3];
          if (values[(int)signalsIdx.h3] < 0 || values[(int)signalsIdx.h3].CompareTo(double.NaN) == 0)
            values[(int)signalsIdx.h3] = 0;
          //h1 = -(a1 * cycle) / (A1 * 1000) * Math.Sqrt(2 * g * h1) + (a3 * cycle *Math.Sqrt(2 * g * h3)) 
          //  / (A1*1000) + (r1 * k1 * V1/100 * cycle) / (A1 * 1000) + poprz_h1;
          values[(int)signalsIdx.h1] = -(values[(int)signalsIdx.a1] * values[(int)signalsIdx.cycle])
              / (values[(int)signalsIdx.A1] * 1000) * Math.Sqrt(2 * values[(int)signalsIdx.g] *
              values[(int)signalsIdx.h1]) + (values[(int)signalsIdx.a3] * values[(int)signalsIdx.cycle]
              * Math.Sqrt(2 * values[(int)signalsIdx.g] * values[(int)signalsIdx.h3]))
              / (values[(int)signalsIdx.A1] * 1000) + (values[(int)signalsIdx.r1] * values[(int)signalsIdx.k1]
              * values[(int)signalsIdx.V1] / 100 * values[(int)signalsIdx.cycle]) / (values[(int)signalsIdx.A1]
              * 1000) + values[(int)signalsIdx.poprz_h1];
          if (values[(int)signalsIdx.h1] < 0 || values[(int)signalsIdx.h1].CompareTo(double.NaN) == 0)
            values[(int)signalsIdx.h1] = 0;
          // h4 = -(a4 * cycle) / (A4 * 1000) * Math.Sqrt(2 * g * h4) + ((1 - r1) * k1 * V1/100 * cycle) 
          //  / (A4 * 1000) + poprz_h4;
          values[(int)signalsIdx.h4] = -(values[(int)signalsIdx.a4] * values[(int)signalsIdx.cycle])
              / (values[(int)signalsIdx.A4] * 1000) * Math.Sqrt(2 * values[(int)signalsIdx.g]
              * values[(int)signalsIdx.h4]) + ((1 - values[(int)signalsIdx.r1]) * values[(int)signalsIdx.k1]
              * values[(int)signalsIdx.V1] / 100 * values[(int)signalsIdx.cycle]) / (values[(int)signalsIdx.A4]
              * 1000) + values[(int)signalsIdx.poprz_h4];
          if (values[(int)signalsIdx.h4] < 0 || values[(int)signalsIdx.h4].CompareTo(double.NaN) == 0)
            values[(int)signalsIdx.h4] = 0;
          // h2 = -(a2 * cycle) / (A2 * 1000) * Math.Sqrt(2 * g * h2) + (a4 * cycle * Math.Sqrt(2 * g * h4)) 
          //  / (A2 * 1000) + (r2 * k2 * V2/100 * cycle) / (A2 * 1000) + poprz_h2;
          values[(int)signalsIdx.h2] = -(values[(int)signalsIdx.a2] * values[(int)signalsIdx.cycle])
              / (values[(int)signalsIdx.A2] * 1000) * Math.Sqrt(2 * values[(int)signalsIdx.g]
              * values[(int)signalsIdx.h2]) + (values[(int)signalsIdx.a4] * values[(int)signalsIdx.cycle] * Math.Sqrt(2 *
              values[(int)signalsIdx.g] * values[(int)signalsIdx.h4])) / (values[(int)signalsIdx.A2] * 1000)
              + (values[(int)signalsIdx.r2] * values[(int)signalsIdx.k2] * values[(int)signalsIdx.V2] / 100
              * values[(int)signalsIdx.cycle]) / (values[(int)signalsIdx.A2] * 1000) +
              values[(int)signalsIdx.poprz_h2];
          if (values[(int)signalsIdx.h2] < 0 || values[(int)signalsIdx.h2].CompareTo(double.NaN) == 0)
            values[(int)signalsIdx.h2] = 0;
          #endregion rownania symulatora
          #region  po oibliczeniach glownych:

          //wyznaczenie rozniczek
          values[(int)signalsIdx.dh1] = values[(int)signalsIdx.h1] - values[(int)signalsIdx.poprz_h1];
          values[(int)signalsIdx.dh2] = values[(int)signalsIdx.h2] - values[(int)signalsIdx.poprz_h2];
          values[(int)signalsIdx.dh3] = values[(int)signalsIdx.h3] - values[(int)signalsIdx.poprz_h3];
          values[(int)signalsIdx.dh4] = values[(int)signalsIdx.h4] - values[(int)signalsIdx.poprz_h4];

          //ustawianie alarmu 
          if (values[(int)signalsIdx.dh1] < values[(int)signalsIdx.alarm_delta] &&
            values[(int)signalsIdx.dh2] < values[(int)signalsIdx.alarm_delta] &&
            values[(int)signalsIdx.dh3] < values[(int)signalsIdx.alarm_delta] &&
            values[(int)signalsIdx.dh4] < values[(int)signalsIdx.alarm_delta]
            )
          {
            valuesCMD[(int)commandsIdx.Alarm_switchON] = false;
            valuesCMD[(int)commandsIdx.Alarm_switchOFF] = true;
          }
          else
          {
            valuesCMD[(int)commandsIdx.Alarm_switchON] = true;
            valuesCMD[(int)commandsIdx.Alarm_switchOFF] = false;
          }

          //poprz_h1=h1
          values[(int)signalsIdx.poprz_h1] = values[(int)signalsIdx.h1];
          //poprz_h2=h2
          values[(int)signalsIdx.poprz_h2] = values[(int)signalsIdx.h2];
          //poprz_h3=h3
          values[(int)signalsIdx.poprz_h3] = values[(int)signalsIdx.h3];
          //poprz_h4=h4
          values[(int)signalsIdx.poprz_h4] = values[(int)signalsIdx.h4];
          #endregion  po oibliczeniach glownych:
          #region Generator
          //generatory
          values[(int)signalsIdx.y_sin] = Generator_sinus(values[(int)signalsIdx.t]);
          values[(int)signalsIdx.y_pil] = Generator_pila(values[(int)signalsIdx.t]);
          if (values[(int)signalsIdx.y_pil] >= (values[(int)signalsIdx.A_pil]) * values[(int)signalsIdx.b])
            values[(int)signalsIdx.mn]++;
          //ustalanie aktualnego czasu lub resetowanie go
          if (valuesCMD[(int)commandsIdx.reset_t] == true)
          {
            values[(int)signalsIdx.t] = 0;
            valuesCMD[(int)commandsIdx.reset_t] = false;
          }
          else
            values[(int)signalsIdx.t] = values[(int)signalsIdx.t] + values[(int)signalsIdx.cycle] / 1000;
          #endregion Generator
          #region toberemoved
          ////jesli nie ma alarmu to zwiekszamy czas probkowania

          //if ( valuesCMD[ (int)commandsIdx.Alarm_switchON ] == false )
          //{
          //  if ( zmiana )
          //  {
          //    values[ (int)signalsIdx.cycle ] = 2 * values[ (int)signalsIdx.cycle ];
          //    zmiana = false;
          //  }

          //}
          //if ( valuesCMD[ (int)commandsIdx.Alarm_switchON ] == true )
          //{
          //  if ( !zmiana )
          //  {
          //    values[ (int)signalsIdx.cycle ] = values[ (int)signalsIdx.cycle ] / 2;
          //    zmiana = true;
          //  }

          //}
          #endregion toberemoved
          #region time
          values[(int)signalsIdx.year] = System.DateTime.Now.Year;
          values[(int)signalsIdx.month] = System.DateTime.Now.Month;
          values[(int)signalsIdx.day] = System.DateTime.Now.Day;
          values[(int)signalsIdx.hour] = System.DateTime.Now.Hour;
          values[(int)signalsIdx.minute] = System.DateTime.Now.Minute;
          values[(int)signalsIdx.second] = System.DateTime.Now.Second;
          #endregion time
          //odczekanie 
          System.Threading.Thread.Sleep((int)values[(int)signalsIdx.cycle]);
        }
        catch (Exception ex)
        {
          EventLogMonitor.WriteToEventLogInfo("Demo simulator has done wrong operation and it was restarted; " + ex.Message,
            (int)CAS.Lib.RTLib.Processes.Error.CommServer_EC2EC3_symulator);
          Initialisation();
        }
      }
    }
    internal enum signalsIdx : int
    {//sygnaly, indexy potrzebne do tablicy values
      h1 = 0,
      h2 = 1,
      h3 = 2,
      h4 = 3,
      V1 = 4,
      V2 = 5,
      a1 = 6,
      a2 = 7,
      a3 = 8,
      a4 = 9,
      A1 = 10,
      A2 = 11,
      A3 = 12,
      A4 = 13,
      r1 = 14,
      r2 = 15,
      k1 = 16,
      k2 = 17,
      poprz_h1 = 18,
      poprz_h2 = 19,
      poprz_h3 = 20,
      poprz_h4 = 21,
      g = 22,
      cycle = 23,
      y_sin = 24,
      y_pil = 25,
      A_sin = 26,
      A_pil = 27,
      t = 28,
      w = 29,
      b = 30,//dlugosc "pily"
      dh1 = 31,
      dh2 = 32,
      dh3 = 33,
      dh4 = 34,
      mn = 35,
      alarm_delta = 36,
      year = 37,
      month = 38,
      day = 39,
      hour = 40,
      minute = 41,
      second = 42,
      TransmissionDelayInMs = 43
    }
    internal enum commandsIdx : int
    {
      ResetAll = 0,  //initialise reset of all values in the simulator
      ResetToCurrent = 1,
      Alarm_switchON = 2,
      Alarm_switchOFF = 3,
      droga_1 = 4,
      droga_2 = 5,
      reset_t = 6,
      droga_1_transmitterON = 7,
      droga_2_transmitterON = 8
    }
    #endregion

  }
}
