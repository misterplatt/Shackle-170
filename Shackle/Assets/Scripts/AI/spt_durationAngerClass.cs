/* spt_durationAngerClass.cs
 * 
 * Created by: Lauren Cunningham
 * 
 * Last Revision Date: 4/18/2016
 * 
 * This is a class that inerits from the base anger class, and is used to track player actions that will continually anger the monster if continued
 *  over a period of time. **/

using UnityEngine;
using System.Collections;

public class spt_durationAngerClass : spt_baseAngerClass {

    public spt_durationAngerClass(int a) :base(a){
        hasDuration = true;
        duration = 0.5f;
        angerIncrease = 1;
    }
}
