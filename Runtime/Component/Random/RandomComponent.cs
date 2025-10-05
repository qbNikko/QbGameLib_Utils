using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace QbGameLib_Utils.Component.Random
{
    public class RandomComponent :MonoBehaviour
    {
        Unity.Mathematics.Random _random;

        public RandomComponent()
        {
            _random = new Unity.Mathematics.Random((uint)DateTimeOffset.Now.ToUnixTimeMilliseconds());
        }

        public void Randomize()
        {
            _random.InitState(_random.NextUInt());
        }
        
        public void SetSeed(uint seed)
        {
            _random.InitState(seed);
        }

        public ref Unity.Mathematics.Random Random => ref _random;
        
        
        public Vector2 GetRandomPositionOnCircle(
            float maxRadius = 1f,
            float minRadius = 0f,
            float minY = float.MinValue
        )
        {
            float a = NextFloat() * 2 * Mathf.PI;
            float r = ((maxRadius-minRadius) * Mathf.Sqrt(NextFloat()))+minRadius;
            return new Vector2((r * Mathf.Cos(a)), MathF.Max(minY,r * Mathf.Sin(a)));
        }
        public Vector2 GetRandomPositionOnRectangle(Rect rect)
        {
            return NextFloat2(rect.min,rect.max);
        }
        
        public Vector3 GetRandomPositionOnBounds(Bounds bounds)
        {
            return NextFloat3(bounds.min,bounds.max);
        }
        
        public T GetRandomElement<T>(List<T> elements)
        {
            return elements[NextInt(0, elements.Count-1)];
        }
        
        public T GetRandomEnum<T>() where T : Enum
        {
            Array values = Enum.GetValues(typeof(T));
            return (T)values.GetValue(NextInt(0, values.Length-1));
        }
        
        
        public bool NextBool()
        {
            return _random.NextBool();
        }

        public bool2 NextBool2()
        {
            return _random.NextBool2();
        }

        public bool3 NextBool3()
        {
            return _random.NextBool3();
        }

        public bool4 NextBool4()
        {
            return _random.NextBool4();
        }

        public int NextInt()
        {
            return _random.NextInt();
        }

        public int2 NextInt2()
        {
            return _random.NextInt2();
        }

        public int3 NextInt3()
        {
            return _random.NextInt3();
        }

        public int4 NextInt4()
        {
            return _random.NextInt4();
        }

        public int NextInt(int max)
        {
            return _random.NextInt(max);
        }

        public int2 NextInt2(int2 max)
        {
            return _random.NextInt2(max);
        }

        public int3 NextInt3(int3 max)
        {
            return _random.NextInt3(max);
        }

        public int4 NextInt4(int4 max)
        {
            return _random.NextInt4(max);
        }

        public int NextInt(int min, int max)
        {
            return _random.NextInt(min, max);
        }

        public int2 NextInt2(int2 min, int2 max)
        {
            return _random.NextInt2(min, max);
        }

        public int3 NextInt3(int3 min, int3 max)
        {
            return _random.NextInt3(min, max);
        }

        public int4 NextInt4(int4 min, int4 max)
        {
            return _random.NextInt4(min, max);
        }

        public uint NextUInt()
        {
            return _random.NextUInt();
        }

        public uint2 NextUInt2()
        {
            return _random.NextUInt2();
        }

        public uint3 NextUInt3()
        {
            return _random.NextUInt3();
        }

        public uint4 NextUInt4()
        {
            return _random.NextUInt4();
        }

        public uint NextUInt(uint max)
        {
            return _random.NextUInt(max);
        }

        public uint2 NextUInt2(uint2 max)
        {
            return _random.NextUInt2(max);
        }

        public uint3 NextUInt3(uint3 max)
        {
            return _random.NextUInt3(max);
        }

        public uint4 NextUInt4(uint4 max)
        {
            return _random.NextUInt4(max);
        }

        public uint NextUInt(uint min, uint max)
        {
            return _random.NextUInt(min, max);
        }

        public uint2 NextUInt2(uint2 min, uint2 max)
        {
            return _random.NextUInt2(min, max);
        }

        public uint3 NextUInt3(uint3 min, uint3 max)
        {
            return _random.NextUInt3(min, max);
        }

        public uint4 NextUInt4(uint4 min, uint4 max)
        {
            return _random.NextUInt4(min, max);
        }

        public float NextFloat()
        {
            return _random.NextFloat();
        }

        public float2 NextFloat2()
        {
            return _random.NextFloat2();
        }

        public float3 NextFloat3()
        {
            return _random.NextFloat3();
        }

        public float4 NextFloat4()
        {
            return _random.NextFloat4();
        }

        public float NextFloat(float max)
        {
            return _random.NextFloat(max);
        }

        public float2 NextFloat2(float2 max)
        {
            return _random.NextFloat2(max);
        }

        public float3 NextFloat3(float3 max)
        {
            return _random.NextFloat3(max);
        }

        public float4 NextFloat4(float4 max)
        {
            return _random.NextFloat4(max);
        }

        public float NextFloat(float min, float max)
        {
            return _random.NextFloat(min, max);
        }

        public float2 NextFloat2(float2 min, float2 max)
        {
            return _random.NextFloat2(min, max);
        }

        public float3 NextFloat3(float3 min, float3 max)
        {
            return _random.NextFloat3(min, max);
        }

        public float4 NextFloat4(float4 min, float4 max)
        {
            return _random.NextFloat4(min, max);
        }

        public double NextDouble()
        {
            return _random.NextDouble();
        }

        public double2 NextDouble2()
        {
            return _random.NextDouble2();
        }

        public double3 NextDouble3()
        {
            return _random.NextDouble3();
        }

        public double4 NextDouble4()
        {
            return _random.NextDouble4();
        }

        public double NextDouble(double max)
        {
            return _random.NextDouble(max);
        }

        public double2 NextDouble2(double2 max)
        {
            return _random.NextDouble2(max);
        }

        public double3 NextDouble3(double3 max)
        {
            return _random.NextDouble3(max);
        }

        public double4 NextDouble4(double4 max)
        {
            return _random.NextDouble4(max);
        }

        public double NextDouble(double min, double max)
        {
            return _random.NextDouble(min, max);
        }

        public double2 NextDouble2(double2 min, double2 max)
        {
            return _random.NextDouble2(min, max);
        }

        public double3 NextDouble3(double3 min, double3 max)
        {
            return _random.NextDouble3(min, max);
        }

        public double4 NextDouble4(double4 min, double4 max)
        {
            return _random.NextDouble4(min, max);
        }

        public float2 NextFloat2Direction()
        {
            return _random.NextFloat2Direction();
        }

        public double2 NextDouble2Direction()
        {
            return _random.NextDouble2Direction();
        }

        public float3 NextFloat3Direction()
        {
            return _random.NextFloat3Direction();
        }

        public double3 NextDouble3Direction()
        {
            return _random.NextDouble3Direction();
        }

        public quaternion NextQuaternionRotation()
        {
            return _random.NextQuaternionRotation();
        }
    }
}