﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyGame.Library.Materials;

namespace MyGame
{
	public class CModel : IRenderable
	{
		public Vector3 Position { get; set; }
		public Vector3 Rotation { get; set; }
		public Vector3 Scale { get; set; }

		public Model Model { get; private set; }

		private Matrix[] modelTransforms;
		private GraphicsDevice graphicsDevice;
		private BoundingSphere boundingSphere;

		public Material Material { get; set; }

		public BoundingSphere BoundingSphere
		{
			get
			{
				// No need for rotation, as this is a sphere
				Matrix worldTransform = Matrix.CreateScale(Scale)
					* Matrix.CreateTranslation(Position);

				BoundingSphere transformed = boundingSphere;
				transformed = transformed.Transform(worldTransform);

				return transformed;
			}
		}

		public bool EnableLighting { get { return enableLighting; } }
		private bool enableLighting;

		public CModel(Model Model, Vector3 Position, Vector3 Rotation,
			Vector3 Scale, GraphicsDevice graphicsDevice) : this(Model, Position, Rotation, Scale, graphicsDevice, true)
		{
		}

		public CModel(Model Model, Vector3 Position, Vector3 Rotation,
			Vector3 Scale, GraphicsDevice graphicsDevice, bool enableLighting)
		{
			this.Model = Model;

			modelTransforms = new Matrix[Model.Bones.Count];
			Model.CopyAbsoluteBoneTransformsTo(modelTransforms);

			buildBoundingSphere();
			generateTags();

			this.Position = Position;
			this.Rotation = Rotation;
			this.Scale = Scale;

			this.graphicsDevice = graphicsDevice;

			this.Material = new Material();
			this.enableLighting = enableLighting;
		}

		private void buildBoundingSphere()
		{
			BoundingSphere sphere = new BoundingSphere(Vector3.Zero, 0);

			// Merge all the model's built in bounding spheres
			foreach (ModelMesh mesh in Model.Meshes)
			{
				BoundingSphere transformed = mesh.BoundingSphere.Transform(
					modelTransforms[mesh.ParentBone.Index]);

				sphere = BoundingSphere.CreateMerged(sphere, transformed);
			}

			this.boundingSphere = sphere;
		}

		public void Draw(Matrix View, Matrix Projection)
		{
			// Calculate the base transformation by combining
			// translation, rotation, and scaling
			Matrix baseWorld = Matrix.CreateScale(Scale)
							   * Matrix.CreateFromYawPitchRoll(
								   Rotation.Y, Rotation.X, Rotation.Z)
							   * Matrix.CreateTranslation(Position);

			foreach (ModelMesh mesh in Model.Meshes)
			{
				Matrix localWorld = modelTransforms[mesh.ParentBone.Index]
									* baseWorld;

				foreach (ModelMeshPart meshPart in mesh.MeshParts)
				{
					BasicEffect effect = (BasicEffect)meshPart.Effect;

					effect.World = localWorld;
					effect.View = View;
					effect.Projection = Projection;

					if (enableLighting)
					{
						effect.EnableDefaultLighting();
					}
				}

				mesh.Draw();
			}
		}

		public void Draw(Matrix View, Matrix Projection, Vector3 CameraPosition)
		{
			// Calculate the base transformation by combining
			// translation, rotation, and scaling
			Matrix baseWorld = Matrix.CreateScale(Scale)
				* Matrix.CreateFromYawPitchRoll(
					Rotation.Y, Rotation.X, Rotation.Z)
				* Matrix.CreateTranslation(Position);

			foreach (ModelMesh mesh in Model.Meshes)
			{
				Matrix localWorld = modelTransforms[mesh.ParentBone.Index]
					* baseWorld;

				foreach (ModelMeshPart meshPart in mesh.MeshParts)
				{
					Effect effect = meshPart.Effect;

					if (effect is BasicEffect)
					{
						((BasicEffect)effect).World = localWorld;
						((BasicEffect)effect).View = View;
						((BasicEffect)effect).Projection = Projection;

						if (enableLighting)
						{
							((BasicEffect)effect).EnableDefaultLighting();
						}
					}
					else
					{
						setEffectParameter(effect, "World", localWorld);
						setEffectParameter(effect, "View", View);
						setEffectParameter(effect, "Projection", Projection);
						setEffectParameter(effect, "CameraPosition", CameraPosition);

						Material.SetEffectParameters(effect);
					}
				}

				mesh.Draw();
			}
		}

		// Sets the specified effect parameter to the given effect, if it
		// has that parameter
		void setEffectParameter(Effect effect, string paramName, object val)
		{
			if (effect.Parameters[paramName] == null)
				return;

			if (val is Vector3)
				effect.Parameters[paramName].SetValue((Vector3)val);
			else if (val is bool)
				effect.Parameters[paramName].SetValue((bool)val);
			else if (val is Matrix)
				effect.Parameters[paramName].SetValue((Matrix)val);
			else if (val is Texture2D)
				effect.Parameters[paramName].SetValue((Texture2D)val);
		}

		public void SetModelEffect(Effect effect, bool CopyEffect)
		{
			foreach (ModelMesh mesh in Model.Meshes)
				foreach (ModelMeshPart part in mesh.MeshParts)
				{
					Effect toSet = effect;

					// Copy the effect if necessary
					if (CopyEffect)
						toSet = effect.Clone();

					MeshTag tag = ((MeshTag)part.Tag);

					// If this ModelMeshPart has a texture, set it to the effect
					if (tag.Texture != null)
					{
						setEffectParameter(toSet, "BasicTexture", tag.Texture);
						setEffectParameter(toSet, "TextureEnabled", true);
					}
					else
						setEffectParameter(toSet, "TextureEnabled", false);

					// Set our remaining parameters to the effect
					setEffectParameter(toSet, "DiffuseColor", tag.Color);
					setEffectParameter(toSet, "SpecularPower", tag.SpecularPower);

					part.Effect = toSet;
				}
		}

		public void SetMeshEffect(string MeshName, Effect effect, bool CopyEffect)
		{
			foreach (ModelMesh mesh in Model.Meshes)
			{
				if (mesh.Name != MeshName)
					continue;

				foreach (ModelMeshPart part in mesh.MeshParts)
				{
					Effect toSet = effect;

					// Copy the effect if necessary
					if (CopyEffect)
						toSet = effect.Clone();

					MeshTag tag = ((MeshTag)part.Tag);

					// If this ModelMeshPart has a texture, set it to the effect
					if (tag.Texture != null)
					{
						setEffectParameter(toSet, "BasicTexture", tag.Texture);
						setEffectParameter(toSet, "TextureEnabled", true);
					}
					else
						setEffectParameter(toSet, "TextureEnabled", false);

					// Set our remaining parameters to the effect
					setEffectParameter(toSet, "DiffuseColor", tag.Color);
					setEffectParameter(toSet, "SpecularPower", tag.SpecularPower);

					part.Effect = toSet;
				}
			}
		}

		public void SetModelMaterial(Material material)
		{
			foreach (ModelMesh mesh in Model.Meshes)
				SetMeshMaterial(mesh.Name, material);
		}

		public void SetMeshMaterial(string MeshName, Material material)
		{
			foreach (ModelMesh mesh in Model.Meshes)
			{
				if (mesh.Name != MeshName)
					continue;

				foreach (ModelMeshPart meshPart in mesh.MeshParts)
					((MeshTag)meshPart.Tag).Material = material;
			}
		}

		private void generateTags()
		{
			foreach (ModelMesh mesh in Model.Meshes)
				foreach (ModelMeshPart part in mesh.MeshParts)
					if (part.Effect is BasicEffect)
					{
						BasicEffect effect = (BasicEffect)part.Effect;
						MeshTag tag = new MeshTag(effect.DiffuseColor,
							effect.Texture, effect.SpecularPower);
						part.Tag = tag;
					}
		}

		// Store references to all of the model's current effects
		public void CacheEffects()
		{
			foreach (ModelMesh mesh in Model.Meshes)
				foreach (ModelMeshPart part in mesh.MeshParts)
					((MeshTag)part.Tag).CachedEffect = part.Effect;
		}

		// Restore the effects referenced by the model's cache
		public void RestoreEffects()
		{
			foreach (ModelMesh mesh in Model.Meshes)
				foreach (ModelMeshPart part in mesh.MeshParts)
					if (((MeshTag)part.Tag).CachedEffect != null)
						part.Effect = ((MeshTag)part.Tag).CachedEffect;
		}

		public void SetClipPlane(Vector4? Plane)
		{
			foreach (ModelMesh mesh in Model.Meshes)
				foreach (ModelMeshPart part in mesh.MeshParts)
				{
					if (part.Effect.Parameters["ClipPlaneEnabled"] != null)
						part.Effect.Parameters["ClipPlaneEnabled"].SetValue(Plane.HasValue);

					if (Plane.HasValue)
						if (part.Effect.Parameters["ClipPlane"] != null)
							part.Effect.Parameters["ClipPlane"].SetValue(Plane.Value);
				}
		}
	}

	public class MeshTag
	{
		public Vector3 Color;
		public Texture2D Texture;
		public float SpecularPower;
		public Effect CachedEffect = null;
		public Material Material = new Material();

		public MeshTag(Vector3 Color, Texture2D Texture, float SpecularPower)
		{
			this.Color = Color;
			this.Texture = Texture;
			this.SpecularPower = SpecularPower;
		}
	}

}
