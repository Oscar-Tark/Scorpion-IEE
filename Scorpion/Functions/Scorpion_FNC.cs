﻿/*  <Scorpion IEE(Intelligent Execution Environment). Server To Run Scorpion Built Applications Using the Scorpion Language>
    Copyright (C) <2020+>  <Oscar Arjun Singh Tark>

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU Affero General Public License as
    published by the Free Software Foundation, either version 3 of the
    License, or (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Affero General Public License for more details.

    You should have received a copy of the GNU Affero General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Collections;
using System.Reflection;

//Static Library
namespace Scorpion
{
    partial class Librarian
    {
        public void listfunctions(ref string Scorp_Line_Exec, ref ArrayList objects)
        {
            string STR_ = "";
            foreach (MethodInfo mi in this.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance))
            {
                STR_ += mi.Name + " [Parameters:";
                foreach (ParameterInfo pi in mi.GetParameters())
                    STR_ += " >" + pi.Name;
                STR_ += "]\n";
            }
            Do_on.write_to_cui(STR_);

            //clean
            var_dispose_internal(ref Scorp_Line_Exec);
            var_arraylist_dispose(ref objects);
            return;
        }

        public void recursive(ref string Scorp_Line_Exec, ref ArrayList objects)
        {
            /*VARIABLES MUST BE {&quot}, {&var} not * or *'' */
            Do_on.tms.add(var_get(objects[0]), var_get(objects[1]));
            var_dispose_internal(ref Scorp_Line_Exec);
            var_arraylist_dispose(ref objects);
            return;
        }

        public void recursivedelete(ref string Scorp_Line_Exec, ref ArrayList objects)
        {
            //::*name
            Do_on.tms.delete(var_get(objects[0]));

            var_dispose_internal(ref Scorp_Line_Exec);
            var_arraylist_dispose(ref objects);
            return;
        }

        public void recursivetime(ref string Scorp_Line_Exec, ref ArrayList objects)
        {
            //::*time_in_ms
            Do_on.tms.change_interval(Convert.ToInt32(var_get(objects[0])));

            var_dispose_internal(ref Scorp_Line_Exec);
            var_arraylist_dispose(ref objects);
            return;
        }

        /*public void importscript(string Scorp_Line_Exec, ArrayList objects)
        {
            //IMPORTS A CUSTOM OR DEFAULT SCRIPT MADE IN C#
            /*(*name,*namespaceclass)*//*
            Do_on.hook.import_assembly(var_get(objects[0].ToString()).ToString());
            Do_on.hook.create_instance((System.Reflection.Assembly)Do_on.AL_ASSEMB[Do_on.AL_ASSEMB_REF.IndexOf(var_get(objects[0].ToString()))], var_get(objects[1].ToString()).ToString(), var_get(objects[0].ToString()).ToString());

            Scorp_Line_Exec = null;
            var_arraylist_dispose(ref objects);

            return;
        }

        public void compilescript(string Scorp_Line_Exec, ArrayList objects)
        {
            //EXAMPLE:
            //compilescript(*"c:\users\oscar\test_files\hello.cs", *"hello", *"hello.hello") 
            
            /*(*@get,*outputname,*namespace.class,*refassembly,*refassembly)
              (hello.cs,hello,nc.nc)
              (hello.cs,hello,nc.nc,*system)

              In order to compile put script in a seperate folder that is not OneSource as it will be copied,
              execute compile command
            *//*
            ArrayList references = new ArrayList();
            for(int i = 3; i < objects.Count; i++)
                references.Add(var_get(objects[i].ToString()));

            string[] paths = Do_on.hook.compile_(var_get(objects[0].ToString()).ToString(), var_get(objects[1].ToString()).ToString(), var_get(objects[2].ToString()).ToString(), ref references);
            Do_on.write_to_cui("Compile process completed\nOriginal file: " + var_get(objects[0].ToString()).ToString() + "\nCode file: " + paths[0] + "\nAssembly: " + paths[1]);

            Scorp_Line_Exec = null;
            var_arraylist_dispose(ref objects);
            var_arraylist_dispose(ref references);
            paths = null;
            return;
        }

        public void compiled_instance(ref string Scorp_Line)
        {
            /*(*name,*namespace.class)*//*
            ArrayList al = cut_variables(ref Scorp_Line);
            Do_on.hook.create_instance((System.Reflection.Assembly)Do_on.AL_ASSEMB[Do_on.AL_ASSEMB_REF.IndexOf(var_get(al[0].ToString()).ToString())], var_get(al[1].ToString()).ToString(), var_get(al[0].ToString()).ToString());

            var_arraylist_dispose(ref al);
            Scorp_Line = null;
            return;
        }

        public void call_compiled_function(ref string Scorp_Line)
        {
            /*(*name,*namespace.class,*function,*arg,*arg,...)*//*
            ArrayList al = cut_variables(ref Scorp_Line);
            Do_on.hook.call_compiled_function(var_get(al[0].ToString()).ToString() ,(System.Reflection.Assembly)Do_on.AL_ASSEMB[Do_on.AL_ASSEMB_REF.IndexOf(var_get(al[0].ToString()).ToString())], var_get(al[1].ToString()).ToString(), var_get(al[2].ToString()).ToString(), "");

            var_arraylist_dispose(ref al);
            Scorp_Line = null;

            return;
        }

        public void Register_Function(string Scorp_Line)
        {
            //[name]action;action;action;
            //[name]*b>>app.wr(*true);app.wr(*false);

            //Get Name
            if (Do_on.AL_Ref_EVT.Contains(Scorp_Line.Remove(Scorp_Line.IndexOf("]")).Remove(0, Scorp_Line.IndexOf("[") + 1)) == false)
            {
                Do_on.AL_Ref_EVT.Add(Scorp_Line.Remove(Scorp_Line.IndexOf("]")).Remove(0, Scorp_Line.IndexOf("[") + 1));
                Do_on.AL_EVT.Add(Scorp_Line.Remove(Scorp_Line.LastIndexOf(")")).Remove(0, Scorp_Line.IndexOf("]", 0) + 1));
            }
            else
            {
                Do_on.AL_EVT[Do_on.AL_Ref_EVT.IndexOf(Scorp_Line.Remove(Scorp_Line.IndexOf("]")).Remove(0, Scorp_Line.IndexOf("[") + 1))] = Scorp_Line.Remove(Scorp_Line.LastIndexOf(")")).Remove(0, Scorp_Line.IndexOf("]", 0) + 1);
            }

            //Clean
            Scorp_Line = null;

            return;
        }

        public void Remove_Function(string Scorp_Line)
        {
            //(*function,*function,...)
            ArrayList al = cut_variables(ref Scorp_Line);
            int ndx;

            foreach (object o in al)
            {
                ndx = Do_on.AL_Ref_EVT.IndexOf(var_cut_symbol(o.ToString()));
       
                Do_on.AL_Ref_EVT.RemoveAt(ndx);
                Do_on.AL_EVT.RemoveAt(ndx);
            }

            var_arraylist_dispose(ref al);
            Scorp_Line = null;

            return;
        }

        public bool Functions_get(string Scorp_Line, int index)
        {
            bool is_func = false; //CHANGE WHEN FUNCTIONS ARE ADDED

            //clean
            Scorp_Line = null;

            return is_func;
        }

        //TESTING POSTPONED - PROCEDURES NOT DEFINED
        /*private void Number_Loop(string Scorp_Line_Exec)
        {
            //(start(*),stop(*),function(*),type(*))

            ArrayList al = cut_variables(ref Scorp_Line_Exec);

            //int keep;
            if (var_get(al[3].ToString()).ToString() == Do_on.types.S_Less)
            {
                for (int i = Convert.ToInt32(var_get(al[0].ToString())); i <= Convert.ToInt32(var_get(al[1].ToString())); i++)
                {
                    call_function(var_get(al[2].ToString()).ToString());
                }
            }
            else
            {
                for (int i = Convert.ToInt32(var_get(al[0].ToString())); i >= Convert.ToInt32(var_get(al[1].ToString())); i++)
                {
                    call_function(var_get(al[2].ToString()).ToString());
                }
            }

            Do_on.path_tmp = null;
            Do_on.func_tmp = null;

            var_arraylist_dispose(ref al);

            return;
        }*/

        //TO BE FINISHED
        /*private void Condition(string Scorp_Group_Exec)
        {
            compare_equal();
            //(resulttovar(*),operator(AND/OR),logical(True/False),function(*),variable(*),variable(*),...)
            ArrayList al = cut_variables(ref Scorp_Group_Exec);
            bool accumulation = false;
            bool result = false;

            for (int i = 4; i < al.Count - 1; i++)
            {
                if (var_get(al[i].ToString()) == var_get(al[0].ToString()))
                {
                    result = true;
                }
                else
                {
                    result = false;
                }

                //Logical P^Q
                if (var_get(al[2].ToString()).ToString() == Do_on.types.S_Yes)
                {
                    if (var_get(al[1].ToString()).ToString() == Do_on.types.S_And)
                    {
                        if (result == true && accumulation == true)
                        {
                            accumulation = true;
                        }
                        else if (result == true && accumulation == false)
                        {
                            accumulation = false;
                        }
                        else if (result == false && accumulation == true)
                        {
                            accumulation = false;
                        }
                        else if (result == false && accumulation == false)
                        {
                            accumulation = false;
                        }
                    }
                    else if (var_get(al[1].ToString()).ToString() == Do_on.types.S_Or)
                    {
                        if (result == true && accumulation == true)
                        {
                            accumulation = true;
                        }
                        else if (result == true && accumulation == false)
                        {
                            accumulation = false;
                        }
                        else if (result == false && accumulation == true)
                        {
                            accumulation = true;
                        }
                        else if (result == false && accumulation == false)
                        {
                            accumulation = true;
                        }
                    }
                }
                //Non Logical
                else if (var_get(al[2].ToString()).ToString() == Do_on.types.S_No)
                {
                    if (result == true)
                    {
                        accumulation = true;
                    }
                    else if (result == false)
                    {
                        accumulation = false;
                    }
                }
            }

            MessageBox.Show(result.ToString());
            //var_set(al[0].ToString(), result.ToString());

            //CLEAN
            var_arraylist_dispose(ref al);

            return;
        }*/
        //General Core


        /*public void call_function(string Line_of_Code)
        {
            bool first = true; int index = 0; int index2 = 0;

            foreach (char c in Do_on.AL_EVT[Do_on.AL_Ref_EVT.IndexOf(Line_of_Code)].ToString())
            {
                //Find Functions and Feed
                //app.wr(*);app.wr(*);app.ext;
                try
                {
                    if (!first)
                    {
                        index = Do_on.AL_EVT[Do_on.AL_Ref_EVT.IndexOf(Line_of_Code)].ToString().IndexOf(";", index);
                    }
                    index2 = Do_on.AL_EVT[Do_on.AL_Ref_EVT.IndexOf(Line_of_Code)].ToString().IndexOf(";", index + 1);

                    if (first)
                    {
                        Do_on.Access_Work(Do_on.AL_EVT[Do_on.AL_Ref_EVT.IndexOf(Line_of_Code)].ToString().Remove(index2));
                        first = false;
                    }
                    else
                    {
                        Do_on.Access_Work(Do_on.AL_EVT[Do_on.AL_Ref_EVT.IndexOf(Line_of_Code)].ToString().Remove(index2).Remove(0, index + 1));
                    }
                    index++;
                }
                catch { break; }
            }

            //clean
            Line_of_Code = null;

            return;
        }*/

        //START THE RCALL TIMERS
        /*public void start_recursive_service()
        {
            foreach(ArrayList al in Do_on.AL_REC)
            {
                ((System.Windows.Forms.Timer)al[0]).Start();
            }
            return;
        }

        public void start_recursive_caller(string Scorp_Line)
        {
            ArrayList al = cut_variables(Scorp_Line);

            foreach (string s in al)
            {
                ((System.Windows.Forms.Timer)((ArrayList)Do_on.AL_REC[Do_on.AL_REC_REF.IndexOf(var_cut_symbol(s))])[0]).Start();
            }

            //clean
            var_arraylist_dispose(ref al);
            Scorp_Line = null;

            return;
        }

        public void stop_recursive_caller(string Scorp_Line)
        {
            ArrayList al = cut_variables(Scorp_Line);

            foreach (string s in al)
            {
                ((System.Windows.Forms.Timer)((ArrayList)Do_on.AL_REC_TME[Do_on.AL_REC_REF.IndexOf(var_cut_symbol(s))])[0]).Stop();
            }

            //clean
            var_arraylist_dispose(ref al);
            Scorp_Line = null;

            return;
        }

        public void stop_recursive_service()
        {
            foreach (ArrayList al in Do_on.AL_REC)
            {
                ((System.Windows.Forms.Timer)al[0]).Stop();
            }
            return;
        }*/

        /*private void do_event(object event_)
        {
            string Scorpion_String = event_.ToString();
            string hold = "";
            int Cursor_Index = 0; int n2 = 0;
            foreach (char c in Scorpion_String)
            {
                try
                {
                    n2 = Scorpion_String.IndexOf("~;", Cursor_Index);

                    hold = Scorpion_String.Remove(n2);
                    hold = hold.Remove(0, Cursor_Index);
                    scorpioniee(hold);
                    Cursor_Index = n2 + 2;
                }
                catch { break; }
            }
            //work_(event_);
        }

        private void do_re_read(string event_)
        {
            Thread th_evt = new Thread(new ParameterizedThreadStart(del_re_read));
            th_evt.IsBackground = true;
            th_evt.Priority = ThreadPriority.AboveNormal;
            th_evt.Start((object)event_);
        }

        delegate void d(object event_);
        private void del_re_read(object event_)
        {
            d d1 = new d(do_event);
            //Do_on.Invoke(d1, event_);
        }*/
    }
}