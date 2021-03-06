﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JeuDeThud
{
    [System.Serializable]
    public class Player
    {

        public int ID { get; set; }
        public string Name { get; set; }
        public string Base64Image { get; set; }

        public Player(string name)
        {
            Name = name;
        }
    }
}

