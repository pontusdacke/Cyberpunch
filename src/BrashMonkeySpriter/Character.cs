﻿/*==========================================================================
 * Project: BrashMonkeySpriter
 * File: Mainline.cs
 *
 *==========================================================================
 * Author:
 * Geoff "NowSayPillow" Lodder
 *==========================================================================*/
/*Small edits by:
 * Antti Lindén
 *==========================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using BrashMonkeySpriter.Spriter;

namespace BrashMonkeySpriter {
    public class CharacterModel : List<Entity> {
        public List<Texture2D> Textures { get; internal protected set; }
        public List<List<Rectangle>> Rectangles { get; internal protected set; }
        public List<CharacterMap> CharacterMaps { get; internal protected set; }

        public CharacterModel()
            : base() {

            Textures = new List<Texture2D>();
            Rectangles = new List<List<Rectangle>>();
        }

        public Entity this[string p_name] {
            get { return this.FirstOrDefault(x => x.Name == p_name); }
        }

        public CharacterAnimator CreateAnimator(String p_entity) {
            return new CharacterAnimator(this, p_entity);
        }
    }

    public class CharacterAnimator {
        public string CurrentAnimation { get; private set; }

        protected struct RenderMatrix {
            public float Alpha;
            public SpriteEffects Effects;
            public int File;
            public int Folder;
            public Vector2 Location;
            public Vector2 Pivot;
            public float Rotation;
            public Vector2 Scale;
            public int ZOrder;

            public RenderMatrix(AnimationTransform p_at) {
                Alpha = p_at.Alpha;
                Location = p_at.Location;
                Rotation = p_at.Rotation;
                Pivot = p_at.Pivot;
                Scale = p_at.Scale;

                Effects = SpriteEffects.None;
                File = 0;
                Folder = 0;
                ZOrder = 0;
            }
        }

        protected struct AnimationTransform {
            public float Alpha;
            public Vector2 Location;
            public Vector2 Pivot;
            public Single Rotation;
            public Vector2 Scale;

            public AnimationTransform(Vector2 p_location, float p_rotation, Vector2 p_pivot, Vector2 p_scale) {
                Alpha = 1.0f;
                Location = p_location;
                Rotation = p_rotation;
                Pivot = p_pivot;
                Scale = p_scale;
            }
        }

        protected Color m_color = Color.White;
        public virtual Color Color {
            get { return m_color; }
            set { m_color = value; }
        }

        protected bool m_flipX = false, m_flipY = false;
        public virtual bool FlipX {
            get { return m_flipX; }
            set { m_flipX = value; }
        }

        public virtual bool FlipY {
            get { return m_flipY; }
            set { m_flipY = value; }
        }

        protected Vector2 m_location = Vector2.Zero;
        public virtual Vector2 Location {
            get { return m_location; }
            set { m_location = value; }
        }

        protected float m_rotate = 0.0f;
        public virtual float Rotation {
            get { return m_rotate; }
            set { m_rotate = value; }
        }

        protected float m_scale = 1.0f;
        public virtual float Scale {
            get { return m_scale; }
            set { m_scale = value; }
        }

        protected string CurrentCharacterMapName
        {
            get;
            set;
        }

        protected CharacterMap CurrentCharacterMap
        {
            get;
            set;
        }


        protected List<CharacterMap> CharacterMapList
        {
            get;
            set;

        }





        protected Entity m_entity = null;
        protected Animation m_current;
        protected int m_elapsedTime = 0;

        protected List<RenderMatrix> m_renderList;
        protected List<Texture2D> m_tx;
        protected List<List<Rectangle>> m_rect;

        protected Dictionary<int, AnimationTransform> m_boneTransforms;

        public delegate void AnimationEndedHandler();
        public event AnimationEndedHandler AnimationEnded;

        public CharacterAnimator(CharacterModel p_model, String p_entity) {
            m_entity = p_model[p_entity];
            m_tx = p_model.Textures;
            m_rect = p_model.Rectangles;
            CharacterMapList = p_model.CharacterMaps;
            ChangeAnimation(0);
        }

        public void ChangeAnimation(String p_name) {
            if (CurrentAnimation != p_name) m_elapsedTime = 0;
            CurrentAnimation = p_name;
            m_current = m_entity[p_name];
            m_renderList = new List<RenderMatrix>(m_current.MainLine[0].Body.Count);
            m_boneTransforms = new Dictionary<int, AnimationTransform>(m_current.MainLine[0].Body.Count);           
        }


        public void ApplyCharMap(string mapName)
        {
           
            var empnamesEnum = from emp in  CharacterMapList
                   where emp.Name == mapName select emp;
            
            CurrentCharacterMap = empnamesEnum.FirstOrDefault();
            if ( CurrentCharacterMap != null )
                CurrentCharacterMapName = CurrentCharacterMap.Name;
        }

        public void RemoveCharacterMap()
        {
            CurrentCharacterMapName = null;
        }


        public void ChangeAnimation(int p_index) {
            m_current = m_entity[p_index];
            m_renderList = new List<RenderMatrix>(m_current.MainLine[0].Body.Count);
            m_boneTransforms = new Dictionary<int, AnimationTransform>(m_current.MainLine[0].Body.Count);
        }

        protected AnimationTransform GetFrameTransition(Reference p_ref) {
            Timeline l_timeline = m_current.TimeLines[p_ref.Timeline];
            
            // Find the current frame. 
            // The one referenced by mainline is not neccesarily the correct one
            // I guess the Spriter editor sometimes messes things up
            // I'm not sure how to reproduce this problem but better safe than sorry? For the reference XSpriter does something similar

            int l_keyCur = l_timeline.KeyAtOrBefore(m_elapsedTime), l_keyNext = 0;
            int l_thisTime = m_elapsedTime - l_timeline.Keys[l_keyCur].Time, l_nextTime = 0;
            // Find the next frame.
            if ((l_keyCur + 1) < l_timeline.Keys.Count) {
                l_keyNext = l_keyCur + 1;
                l_nextTime = l_timeline.Keys[l_keyNext].Time;
            } else if (m_current.Looping) {
                // Assume that there is a frame at time = 0
                l_keyNext = 0;
                l_nextTime = m_current.Length;
            } else {
                l_keyNext = l_keyCur;
                l_nextTime = m_current.Length;
            }

            //  Figure out where we are in the timeline...
            l_nextTime = l_nextTime - l_timeline.Keys[l_keyCur].Time;

            TimelineKey l_now = l_timeline.Keys[l_keyCur], l_next = l_timeline.Keys[l_keyNext];
            float l_timeRatio = MathHelper.Clamp((float)l_thisTime / (float)l_nextTime, 0.0f, 1.0f);

            /// Tween EVERYTHING... Gonna have to add an option for it not to...
            /// Rotations are handled differently depending on which way they're supposed to spin
            AnimationTransform l_render = new AnimationTransform();

            float l_angleA = l_now.Rotation, l_angleB = l_next.Rotation;
            if (l_now.Spin == SpinDirection.None) {
                l_angleA = l_angleB = l_now.Rotation;
            } else if (l_now.Spin == SpinDirection.Clockwise) {
                if ((l_angleB - l_angleA) < 0.0f) {
                    l_angleB += MathHelper.TwoPi;
                } else {
                    l_angleA %= MathHelper.TwoPi;
                    l_angleB %= MathHelper.TwoPi;
                }
            } else if (l_now.Spin == SpinDirection.CounterClockwise) {
                if ((l_angleB - l_angleA) > 0.0f) {
                    l_angleB -= MathHelper.TwoPi;
                } else {
                    l_angleA %= MathHelper.TwoPi;
                    l_angleB %= MathHelper.TwoPi;
                }
            }

            l_render.Rotation = MathHelper.Lerp(l_angleA, l_angleB, l_timeRatio);
            l_render.Scale = Vector2.Lerp(l_now.Scale, l_next.Scale, l_timeRatio);
            l_render.Location = Vector2.Lerp(l_now.Location, l_next.Location, l_timeRatio);
            l_render.Pivot = Vector2.Lerp(l_now.Pivot, l_next.Pivot, l_timeRatio);
            l_render.Alpha = MathHelper.Lerp(l_now.Alpha, l_next.Alpha, l_timeRatio);

            // So, how far are we between frames?
            return l_render;
        }

        protected AnimationTransform ApplyTransform(AnimationTransform p_transform, AnimationTransform p_baseTransform) {
            //  Create a tranformation matrix so we can find out the location of the bone \ body
            Matrix l_matrix =   Matrix.CreateScale(p_baseTransform.Scale.X, p_baseTransform.Scale.Y, 0) *
                                Matrix.CreateRotationZ(p_baseTransform.Rotation) *
                                Matrix.CreateTranslation(p_baseTransform.Location.X, p_baseTransform.Location.Y, 0);

            AnimationTransform l_result = new AnimationTransform();

            //  Apply the scaling, rotation and tranform matrix to current structure
            l_result.Scale = p_transform.Scale * p_baseTransform.Scale;
            l_result.Rotation = p_transform.Rotation + p_baseTransform.Rotation;
            l_result.Location = Vector2.Transform(p_transform.Location, l_matrix);
            l_result.Alpha = p_transform.Alpha * p_baseTransform.Alpha;
            l_result.Pivot = p_transform.Pivot;

            return l_result;
        }

        protected AnimationTransform ApplyBoneTransforms(MainlineKey p_main, Reference p_reference) {
            if ((p_reference.BoneId >= 0) && m_boneTransforms.ContainsKey(p_reference.BoneId)) {
                return m_boneTransforms[p_reference.BoneId];
            }

            AnimationTransform l_baseTransform;
            if((p_reference.Parent != -1)){
                l_baseTransform = ApplyBoneTransforms(p_main, p_main.Bones[p_reference.Parent]);
            } else {
                //Apply global transforms to objects without parents (location is added later)
                l_baseTransform = new AnimationTransform(Vector2.Zero, Rotation, Vector2.Zero, new Vector2(Math.Abs(Scale)));
            }
            AnimationTransform l_transform = ApplyTransform(GetFrameTransition(p_reference), l_baseTransform);

            if (p_reference.BoneId >= 0) {
                m_boneTransforms.Add(p_reference.BoneId, l_transform);
            }

            return l_transform;
        }



        private void FindMapChar(int folder, int file, out int targetForder, out int targetFile)
        {
            targetForder = folder;
            targetFile = file;

            


            var t = CurrentCharacterMap.Maps.Where(p => p.Folder == folder && p.File == file);

            if (t.Count() > 0)
            {
                var element = t.First();
                targetForder = element.TargetFolder;
                targetFile = element.TargetFile;
            }

        }




        public void Update(GameTime p_gameTime) {
            m_renderList.Clear();
            m_boneTransforms.Clear();
            
            m_elapsedTime += p_gameTime.ElapsedGameTime.Milliseconds;
            if (m_elapsedTime > m_current.Length) {
                if (AnimationEnded != null)
                    AnimationEnded();
                if (m_current.Looping) {
                    m_elapsedTime -= m_current.Length;
                } else {
                    m_elapsedTime = m_current.Length-1;
                }
            }

            int l_frame = 0;
            for (int l_i = 0; (l_i < m_current.MainLine.Count); l_i++) {
                if (m_elapsedTime >= m_current.MainLine[l_i].Time) {
                    l_frame = l_i;
                }
            }

            Vector2 l_flip = new Vector2(m_flipX ? -1.0f : 1.0f, m_flipY ? -1.0f : 1.0f);
            MainlineKey l_mainline = m_current.MainLine[l_frame];
            
            for (int l_i = 0; l_i < l_mainline.Body.Count; l_i++) {
                TimelineKey l_key = m_current.TimeLines[l_mainline.Body[l_i].Timeline].Keys[l_mainline.Body[l_i].Key];
                // check if file for this object is missing, and if so skip calculating transforms
                if (m_rect[l_key.Folder][l_key.File].Width == 0) {
                    continue;
                }

                RenderMatrix l_render = new RenderMatrix(ApplyBoneTransforms(l_mainline, l_mainline.Body[l_i]));


                
                l_render.File = l_key.File;
                l_render.Folder = l_key.Folder;

                if (CurrentCharacterMapName != null)
                {
                    FindMapChar(l_key.Folder, l_key.File, out l_render.Folder, out l_render.File);
                }

                
                
                l_render.Location = Location + Vector2.Multiply(l_render.Location, l_flip);
                    
                if (m_flipX) {
                    l_render.Effects |= SpriteEffects.FlipHorizontally;
                    l_render.Pivot.X = m_rect[l_key.Folder][l_render.File].Width - l_render.Pivot.X;
                }

                if (m_flipY) {
                    l_render.Effects |= SpriteEffects.FlipVertically;
                    l_render.Pivot.Y = m_rect[l_key.Folder][l_render.File].Height - l_render.Pivot.Y;
                }

                if (m_flipX != m_flipY) {
                    l_render.Rotation *= -1.0f;
                }

                l_render.ZOrder = l_mainline.Body[l_i].ZOrder;

                m_renderList.Add(l_render);
            }
        }

        public void Draw(SpriteBatch p_spriteBatch) {
            foreach (RenderMatrix l_render in m_renderList) {
                p_spriteBatch.Draw(
                    m_tx[l_render.Folder],
                    l_render.Location,
                    m_rect[l_render.Folder][l_render.File],
                    m_color * l_render.Alpha,
                    l_render.Rotation,
                    l_render.Pivot,
                    l_render.Scale,
                    l_render.Effects,
                    /*(float)l_render.ZOrder*/0.0f
                );
            }
        }
    }
}
